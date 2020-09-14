//THIS FILE IS AUTOGENERATED BY GHOSTCOMPILER. DON'T MODIFY OR ALTER.
using System;
using AOT;
using Unity.Burst;
using Unity.Networking.Transport;
using Unity.NetCode.LowLevel.Unsafe;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Collections;
using Unity.NetCode;
using Unity.Transforms;
using Unity.Mathematics;
using PropHunt.Mixed.Components;

namespace PropHunt.Generated
{
    [BurstCompile]
    public struct PropHuntMixedComponentsKCCGravityGhostComponentSerializer
    {
        static PropHuntMixedComponentsKCCGravityGhostComponentSerializer()
        {
            State = new GhostComponentSerializer.State
            {
                GhostFieldsHash = 17884505960735121,
                ExcludeFromComponentCollectionHash = 0,
                ComponentType = ComponentType.ReadWrite<PropHunt.Mixed.Components.KCCGravity>(),
                ComponentSize = UnsafeUtility.SizeOf<PropHunt.Mixed.Components.KCCGravity>(),
                SnapshotSize = UnsafeUtility.SizeOf<Snapshot>(),
                ChangeMaskBits = ChangeMaskBits,
                SendMask = GhostComponentSerializer.SendMask.Interpolated | GhostComponentSerializer.SendMask.Predicted,
                SendForChildEntities = 1,
                CopyToSnapshot =
                    new PortableFunctionPointer<GhostComponentSerializer.CopyToFromSnapshotDelegate>(CopyToSnapshot),
                CopyFromSnapshot =
                    new PortableFunctionPointer<GhostComponentSerializer.CopyToFromSnapshotDelegate>(CopyFromSnapshot),
                RestoreFromBackup =
                    new PortableFunctionPointer<GhostComponentSerializer.RestoreFromBackupDelegate>(RestoreFromBackup),
                PredictDelta = new PortableFunctionPointer<GhostComponentSerializer.PredictDeltaDelegate>(PredictDelta),
                CalculateChangeMask =
                    new PortableFunctionPointer<GhostComponentSerializer.CalculateChangeMaskDelegate>(
                        CalculateChangeMask),
                Serialize = new PortableFunctionPointer<GhostComponentSerializer.SerializeDelegate>(Serialize),
                Deserialize = new PortableFunctionPointer<GhostComponentSerializer.DeserializeDelegate>(Deserialize),
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                ReportPredictionErrors = new PortableFunctionPointer<GhostComponentSerializer.ReportPredictionErrorsDelegate>(ReportPredictionErrors),
                #endif
            };
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            State.NumPredictionErrorNames = GetPredictionErrorNames(ref State.PredictionErrorNames);
            #endif
        }
        public static readonly GhostComponentSerializer.State State;
        public struct Snapshot
        {
            public int gravityAcceleration_x;
            public int gravityAcceleration_y;
            public int gravityAcceleration_z;
        }
        public const int ChangeMaskBits = 1;
        [BurstCompile]
        [MonoPInvokeCallback(typeof(GhostComponentSerializer.CopyToFromSnapshotDelegate))]
        private static void CopyToSnapshot(IntPtr stateData, IntPtr snapshotData, int snapshotOffset, int snapshotStride, IntPtr componentData, int componentStride, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                ref var snapshot = ref GhostComponentSerializer.TypeCast<Snapshot>(snapshotData, snapshotOffset + snapshotStride*i);
                ref var component = ref GhostComponentSerializer.TypeCast<PropHunt.Mixed.Components.KCCGravity>(componentData, componentStride*i);
                ref var serializerState = ref GhostComponentSerializer.TypeCast<GhostSerializerState>(stateData, 0);
                snapshot.gravityAcceleration_x = (int) math.round(component.gravityAcceleration.x * 100);
                snapshot.gravityAcceleration_y = (int) math.round(component.gravityAcceleration.y * 100);
                snapshot.gravityAcceleration_z = (int) math.round(component.gravityAcceleration.z * 100);
            }
        }
        [BurstCompile]
        [MonoPInvokeCallback(typeof(GhostComponentSerializer.CopyToFromSnapshotDelegate))]
        private static void CopyFromSnapshot(IntPtr stateData, IntPtr snapshotData, int snapshotOffset, int snapshotStride, IntPtr componentData, int componentStride, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                ref var snapshotInterpolationData = ref GhostComponentSerializer.TypeCast<SnapshotData.DataAtTick>(snapshotData, snapshotStride*i);
                ref var snapshotBefore = ref GhostComponentSerializer.TypeCast<Snapshot>(snapshotInterpolationData.SnapshotBefore, snapshotOffset);
                ref var snapshotAfter = ref GhostComponentSerializer.TypeCast<Snapshot>(snapshotInterpolationData.SnapshotAfter, snapshotOffset);
                float snapshotInterpolationFactor = snapshotInterpolationData.InterpolationFactor;
                ref var component = ref GhostComponentSerializer.TypeCast<PropHunt.Mixed.Components.KCCGravity>(componentData, componentStride*i);
                var deserializerState = GhostComponentSerializer.TypeCast<GhostDeserializerState>(stateData, 0);
                deserializerState.SnapshotTick = snapshotInterpolationData.Tick;
                component.gravityAcceleration = math.lerp(
                    new float3(snapshotBefore.gravityAcceleration_x * 0.01f, snapshotBefore.gravityAcceleration_y * 0.01f, snapshotBefore.gravityAcceleration_z * 0.01f),
                    new float3(snapshotAfter.gravityAcceleration_x * 0.01f, snapshotAfter.gravityAcceleration_y * 0.01f, snapshotAfter.gravityAcceleration_z * 0.01f),
                    snapshotInterpolationFactor);
            }
        }
        [BurstCompile]
        [MonoPInvokeCallback(typeof(GhostComponentSerializer.RestoreFromBackupDelegate))]
        private static void RestoreFromBackup(IntPtr componentData, IntPtr backupData)
        {
            ref var component = ref GhostComponentSerializer.TypeCast<PropHunt.Mixed.Components.KCCGravity>(componentData, 0);
            ref var backup = ref GhostComponentSerializer.TypeCast<PropHunt.Mixed.Components.KCCGravity>(backupData, 0);
            component.gravityAcceleration.x = backup.gravityAcceleration.x;
            component.gravityAcceleration.y = backup.gravityAcceleration.y;
            component.gravityAcceleration.z = backup.gravityAcceleration.z;
        }

        [BurstCompile]
        [MonoPInvokeCallback(typeof(GhostComponentSerializer.PredictDeltaDelegate))]
        private static void PredictDelta(IntPtr snapshotData, IntPtr baseline1Data, IntPtr baseline2Data, ref GhostDeltaPredictor predictor)
        {
            ref var snapshot = ref GhostComponentSerializer.TypeCast<Snapshot>(snapshotData);
            ref var baseline1 = ref GhostComponentSerializer.TypeCast<Snapshot>(baseline1Data);
            ref var baseline2 = ref GhostComponentSerializer.TypeCast<Snapshot>(baseline2Data);
            snapshot.gravityAcceleration_x = predictor.PredictInt(snapshot.gravityAcceleration_x, baseline1.gravityAcceleration_x, baseline2.gravityAcceleration_x);
            snapshot.gravityAcceleration_y = predictor.PredictInt(snapshot.gravityAcceleration_y, baseline1.gravityAcceleration_y, baseline2.gravityAcceleration_y);
            snapshot.gravityAcceleration_z = predictor.PredictInt(snapshot.gravityAcceleration_z, baseline1.gravityAcceleration_z, baseline2.gravityAcceleration_z);
        }
        [BurstCompile]
        [MonoPInvokeCallback(typeof(GhostComponentSerializer.CalculateChangeMaskDelegate))]
        private static void CalculateChangeMask(IntPtr snapshotData, IntPtr baselineData, IntPtr bits, int startOffset)
        {
            ref var snapshot = ref GhostComponentSerializer.TypeCast<Snapshot>(snapshotData);
            ref var baseline = ref GhostComponentSerializer.TypeCast<Snapshot>(baselineData);
            uint changeMask;
            changeMask = (snapshot.gravityAcceleration_x != baseline.gravityAcceleration_x) ? 1u : 0;
            changeMask |= (snapshot.gravityAcceleration_y != baseline.gravityAcceleration_y) ? (1u<<0) : 0;
            changeMask |= (snapshot.gravityAcceleration_z != baseline.gravityAcceleration_z) ? (1u<<0) : 0;
            GhostComponentSerializer.CopyToChangeMask(bits, changeMask, startOffset, 1);
        }
        [BurstCompile]
        [MonoPInvokeCallback(typeof(GhostComponentSerializer.SerializeDelegate))]
        private static void Serialize(IntPtr snapshotData, IntPtr baselineData, ref DataStreamWriter writer, ref NetworkCompressionModel compressionModel, IntPtr changeMaskData, int startOffset)
        {
            ref var snapshot = ref GhostComponentSerializer.TypeCast<Snapshot>(snapshotData);
            ref var baseline = ref GhostComponentSerializer.TypeCast<Snapshot>(baselineData);
            uint changeMask = GhostComponentSerializer.CopyFromChangeMask(changeMaskData, startOffset, ChangeMaskBits);
            if ((changeMask & (1 << 0)) != 0)
                writer.WritePackedIntDelta(snapshot.gravityAcceleration_x, baseline.gravityAcceleration_x, compressionModel);
            if ((changeMask & (1 << 0)) != 0)
                writer.WritePackedIntDelta(snapshot.gravityAcceleration_y, baseline.gravityAcceleration_y, compressionModel);
            if ((changeMask & (1 << 0)) != 0)
                writer.WritePackedIntDelta(snapshot.gravityAcceleration_z, baseline.gravityAcceleration_z, compressionModel);
        }
        [BurstCompile]
        [MonoPInvokeCallback(typeof(GhostComponentSerializer.DeserializeDelegate))]
        private static void Deserialize(IntPtr snapshotData, IntPtr baselineData, ref DataStreamReader reader, ref NetworkCompressionModel compressionModel, IntPtr changeMaskData, int startOffset)
        {
            ref var snapshot = ref GhostComponentSerializer.TypeCast<Snapshot>(snapshotData);
            ref var baseline = ref GhostComponentSerializer.TypeCast<Snapshot>(baselineData);
            uint changeMask = GhostComponentSerializer.CopyFromChangeMask(changeMaskData, startOffset, ChangeMaskBits);
            if ((changeMask & (1 << 0)) != 0)
                snapshot.gravityAcceleration_x = reader.ReadPackedIntDelta(baseline.gravityAcceleration_x, compressionModel);
            else
                snapshot.gravityAcceleration_x = baseline.gravityAcceleration_x;
            if ((changeMask & (1 << 0)) != 0)
                snapshot.gravityAcceleration_y = reader.ReadPackedIntDelta(baseline.gravityAcceleration_y, compressionModel);
            else
                snapshot.gravityAcceleration_y = baseline.gravityAcceleration_y;
            if ((changeMask & (1 << 0)) != 0)
                snapshot.gravityAcceleration_z = reader.ReadPackedIntDelta(baseline.gravityAcceleration_z, compressionModel);
            else
                snapshot.gravityAcceleration_z = baseline.gravityAcceleration_z;
        }
        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        [BurstCompile]
        [MonoPInvokeCallback(typeof(GhostComponentSerializer.ReportPredictionErrorsDelegate))]
        private static void ReportPredictionErrors(IntPtr componentData, IntPtr backupData, ref UnsafeList<float> errors)
        {
            ref var component = ref GhostComponentSerializer.TypeCast<PropHunt.Mixed.Components.KCCGravity>(componentData, 0);
            ref var backup = ref GhostComponentSerializer.TypeCast<PropHunt.Mixed.Components.KCCGravity>(backupData, 0);
            int errorIndex = 0;
            errors[errorIndex] = math.max(errors[errorIndex], math.distance(component.gravityAcceleration, backup.gravityAcceleration));
            ++errorIndex;
        }
        private static int GetPredictionErrorNames(ref FixedString512 names)
        {
            int nameCount = 0;
            if (nameCount != 0)
                names.Append(new FixedString32(","));
            names.Append(new FixedString64("gravityAcceleration"));
            ++nameCount;
            return nameCount;
        }
        #endif
    }
}