using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Deform
{
    [Deformer(Name = "Rolling", Description = "Roll the object", Type = typeof(RollingDeformer))]
    public class RollingDeformer : Deformer
    {
        public float4 UpDir;
        public float MeshTop;
        public float4 RollDir;
        public half Radius;
        public half Deviation;
        public half PointX;
        public half PointY;
        public override DataFlags DataFlags => DataFlags.Vertices;
        public override JobHandle Process(MeshData data, JobHandle dependency = default)
        {
            var job = new RollingJob()
            {
                _UpDir = UpDir,
                _MeshTop = MeshTop,
                _RollDir = RollDir,
                _Radius = Radius,
                _Deviation = Deviation,
                _PointX = PointX,
                _PointY = PointY,
                vertices = data.DynamicNative.VertexBuffer
            };

            var handle = job.Schedule(data.Length, 128, dependency);

            return handle;
        }
        
        [BurstCompile]
        private struct RollingJob : IJobParallelFor
        {
            public NativeArray<float3> vertices;
            public float4 _UpDir;
            public float _MeshTop;
            public float4 _RollDir;
            public half _Radius;
            public half _Deviation;
            public half _PointX;
            public half _PointY;

            public void Execute(int index)
            {
                float3 v0 = vertices[index].xyz;

                float3 upDir = math.normalize(_UpDir).xyz;
                float3 rollDir = math.normalize(_RollDir).xyz;
                
                float y = _PointY;
                float dP = math.dot(v0 - upDir * y, upDir);
                dP = math.max(0, dP);
                float3 fromInitialPos = upDir * dP;
                v0 -= fromInitialPos;

                float radius = _Radius + _Deviation * math.max(0, -(y - _MeshTop));
                float length = 2 * math.PI * (radius - _Deviation * math.max(0, -(y - _MeshTop)) / 2);
                float r = dP / math.max(0, length);
                float a = 2 * r * math.PI;
            
                float s = math.sin(a);
                float c = math.cos(a);
                float one_minus_c = 1.0f - c;

                float3 axis = math.normalize(math.cross(upDir, rollDir));
                float3x3 rot_mat = math.float3x3(
                    one_minus_c * axis.x * axis.x + c, one_minus_c * axis.x * axis.y - axis.z * s, one_minus_c * axis.z * axis.x + axis.y * s,
                    one_minus_c * axis.x * axis.y + axis.z * s, one_minus_c * axis.y * axis.y + c, one_minus_c * axis.y * axis.z - axis.x * s,
                    one_minus_c * axis.z * axis.x - axis.y * s, one_minus_c * axis.y * axis.z + axis.x * s, one_minus_c * axis.z * axis.z + c
                );
                float3 cycleCenter = rollDir * _PointX + rollDir * radius + upDir * y;

                float3 fromCenter = v0.xyz - cycleCenter;
                float3 shiftFromCenterAxis = math.cross(axis, fromCenter);
                shiftFromCenterAxis = math.cross(shiftFromCenterAxis, axis);
                shiftFromCenterAxis = math.normalize(shiftFromCenterAxis);
                fromCenter -= shiftFromCenterAxis * _Deviation * dP;// * ;

                v0.xyz = math.mul(rot_mat, fromCenter) + cycleCenter;

                vertices[index] = v0;
            }
        }
    }
}

