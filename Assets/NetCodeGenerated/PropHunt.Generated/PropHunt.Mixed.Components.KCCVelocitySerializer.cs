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
    public struct PropHuntMixedComponentsKCCVelocityGhostComponentSerializer
    {
        static PropHuntMixedComponentsKCCVelocityGhostComponentSerializer()
        {
            State = new GhostComponentSerializer.State
            {
                GhostFieldsHash = 6104341673800988698,
                ExcludeFromComponentCollectionHash = 0,
                ComponentType = ComponentType.ReadWrite<PropHunt.Mixed.Components.KCCVelocity>(),
                ComponentSize = UnsafeUtility.SizeOf<PropHunt.Mixed.Components.KCCVelocity>(),
                SnapshotSize = UnsafeUtility.SizeOf<Snapshot>(),
                ChangeMaskBits = ChangeMaskBits,
                SendMask = GhostComponentSerializer.SendMask.Predicted,
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
            public int playerVelocity_x;
            public int playerVelocity_y;
            public int playerVelocity_z;
            public int worldVelocity_x;
            public int worldVelocity_y;
            public int worldVelocity_z;
        }
        public const int ChangeMaskBits = 2;
        [BurstCompile]
        [MonoPInvokeCallback(typeof(GhostComponentSerializer.CopyToFromSnapshotDelegate))]
        private static void CopyToSnapshot(IntPtr stateData, IntPtr snapshotData, int snapshotOffset, int snapshotStride, IntPtr componentData, int componentStride, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                ref var snapshot = ref GhostComponentSerializer.TypeCast<Snapshot>(snapshotData, snapshotOffset + snapshotStride*i);
                ref var component = ref GhostComponentSerializer.TypeCast<PropHunt.Mixed.Components.KCCVelocity>(componentData, componentStride*i);
                ref var serializerState = ref GhostComponentSerializer.TypeCast<GhostSerializerState>(stateData, 0);
                snapshot.playerVelocity_x = (int) math.round(component.playerVelocity.x * 100);
                snapshot.playerVelocity_y = (int) math.round(component.playerVelocity.y * 100);
                snapshot.playerVelocity_z = (int) math.round(component.playerVelocity.z * 100);
                snapshot.worldVelocity_x = (int) math.round(component.worldVelocity.x * 100);
                snapshot.worldVelocity_y = (int) math.round(component.worldVelocity.y * 100);
                snapshot.worldVelocity_z = (int) math.round(component.worldVelocity.z * 100);
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
                ref var component = ref GhostComponentSerializer.TypeCast<PropHunt.Mixed.Components.KCCVelocity>(componentData, componentStride*i);
                var deserializerState = GhostComponentSerializer.TypeCast<GhostDeserializerState>(stateData, 0);
                deserializerState.SnapshotTick = snapshotInterpolationData.Tick;
                component.playerVelocity = math.lerp(
                    new float3(snapshotBefore.playerVelocity_x * 0.01f, snapshotBefore.playerVelocity_y * 0.01f, snapshotBefore.playerVelocity_z * 0.01f),
                    new float3(snapshotAfter.playerVelocity_x * 0.01f, snapshotAfter.playerVelocity_y * 0.01f, snapshotAfter.playerVelocity_z * 0.01f),
                    snapshotInterpolationFactor);
                component.worldVelocity = math.lerp(
                    new float3(snapshotBefore.worldVelocity_x * 0.01f, snapshotBefore.worldVelocity_y * 0.01f, snapshotBefore.worldVelocity_z * 0.01f),
                    new float3(snapshotAfter.worldVelocity_x * 0.01f, snapshotAfter.worldVelocity_y * 0.01f, snapshotAfter.worldVelocity_z * 0.01f),
                    snapshotInterpolationFactor);
            }
        }
        [BurstCompile]
        [MonoPInvokeCallback(typeof(GhostComponentSerializer.RestoreFromBackupDelegate))]
        private static void RestoreFromBackup(IntPtr componentData, IntPtr backupData)
        {
            ref var component = ref GhostComponentSerializer.TypeCast<PropHunt.Mixed.Components.KCCVelocity>(componentData, 0);
            ref var backup = ref GhostComponentSerializer.TypeCast<PropHunt.Mixed.Components.KCCVelocity>(backupData, 0);
            component.playerVelocity.x = backup.playerVelocity.x;
            component.playerVelocity.y = backup.playerVelocity.y;
            component.playerVelocity.z = backup.playerVelocity.z;
            component.worldVelocity.x = backup.worldVelocity.x;
            component.worldVelocity.y = backup.worldVelocity.y;
            component.worldVelocity.z = backup.worldVelocity.z;
        }

        [BurstCompile]
        [MonoPInvokeCallback(typeof(GhostComponentSerializer.PredictDeltaDelegate))]
        private static void PredictDelta(IntPtr snapshotData, IntPtr baseline1Data, IntPtr baseline2Data, ref GhostDeltaPredictor predictor)
        {
            ref var snapshot = ref GhostComponentSerializer.TypeCast<Snapshot>(snapshotData);
            ref var baseline1 = ref GhostComponentSerializer.TypeCast<Snapshot>(baseline1Data);
            ref var baseline2 = ref GhostComponentSerializer.TypeCast<Snapshot>(baseline2Data);
            snapshot.playerVelocity_x = predictor.PredictInt(snapshot.playerVelocity_x, baseline1.playerVelocity_x, baseline2.playerVelocity_x);
            snapshot.playerVelocity_y = predictor.PredictInt(snapshot.playerVelocity_y, baseline1.playerVelocity_y, baseline2.playerVelocity_y);
            snapshot.playerVelocity_z = predictor.PredictInt(snapshot.playerVelocity_z, baseline1.playerVelocity_z, baseline2.playerVelocity_z);
            snapshot.worldVelocity_x = predictor.PredictInt(snapshot.worldVelocity_x, baseline1.worldVelocity_x, baseline2.worldVelocity_x);
            snapshot.worldVelocity_y = predictor.PredictInt(snapshot.worldVelocity_y, baseline1.worldVelocity_y, baseline2.worldVelocity_y);
            snapshot.worldVelocity_z = predictor.PredictInt(snapshot.worldVelocity_z, baseline1.worldVelocity_z, baseline2.worldVelocity_z);
        }
        [BurstCompile]
        [MonoPInvokeCallback(typeof(GhostComponentSerializer.CalculateChangeMaskDelegate))]
        private static void CalculateChangeMask(IntPtr snapshotData, IntPtr baselineData, IntPtr bits, int startOffset)
        {
            ref var snapshot = ref GhostComponentSerializer.TypeCast<Snapshot>(snapshotData);
            ref var baseline = ref GhostComponentSerializer.TypeCast<Snapshot>(baselineData);
            uint changeMask;
            changeMask = (snapshot.playerVelocity_x != baseline.playerVelocity_x) ? 1u : 0;
            changeMask |= (snapshot.playerVelocity_y != baseline.playerVelocity_y) ? (1u<<0) : 0;
            changeMask |= (snapshot.playerVelocity_z != baseline.playerVelocity_z) ? (1u<<0) : 0;
            changeMask |= (snapshot.worldVelocity_x != baseline.worldVelocity_x) ? (1u<<1) : 0;
            changeMask |= (snapshot.worldVelocity_y != baseline.worldVelocity_y) ? (1u<<1) : 0;
            changeMask |= (snapshot.worldVelocity_z != baseline.worldVelocity_z) ? (1u<<1) : 0;
            GhostComponentSerializer.CopyToChangeMask(bits, changeMask, startOffset, 2);
        }
        [BurstCompile]
        [MonoPInvokeCallback(typeof(GhostComponentSerializer.SerializeDelegate))]
        private static void Serialize(IntPtr snapshotData, IntPtr baselineData, ref DataStreamWriter writer, ref NetworkCompressionModel compressionModel, IntPtr changeMaskData, int startOffset)
        {
            ref var snapshot = ref GhostComponentSerializer.TypeCast<Snapshot>(snapshotData);
            ref var baseline = ref GhostComponentSerializer.TypeCast<Snapshot>(baselineData);
            uint changeMask = GhostComponentSerializer.CopyFromChangeMask(changeMaskData, startOffset, ChangeMaskBits);
            if ((changeMask & (1 << 0)) != 0)
                writer.WritePackedIntDelta(snapshot.playerVelocity_x, baseline.playerVelocity_x, compressionModel);
            if ((changeMask & (1 << 0)) != 0)
                writer.WritePackedIntDelta(snapshot.playerVelocity_y, baseline.playerVelocity_y, compressionModel);
            if ((changeMask & (1 << 0)) != 0)
                writer.WritePackedIntDelta(snapshot.playerVelocity_z, baseline.playerVelocity_z, compressionModel);
            if ((changeMask & (1 << 1)) != 0)
                writer.WritePackedIntDelta(snapshot.worldVelocity_x, baseline.worldVelocity_x, compressionModel);
            if ((changeMask & (1 << 1)) != 0)
                writer.WritePackedIntDelta(snapshot.worldVelocity_y, baseline.worldVelocity_y, compressionModel);
            if ((changeMask & (1 << 1)) != 0)
                writer.WritePackedIntDelta(snapshot.worldVelocity_z, baseline.worldVelocity_z, compressionModel);
        }
        [BurstCompile]
        [MonoPInvokeCallback(typeof(GhostComponentSerializer.DeserializeDelegate))]
        private static void Deserialize(IntPtr snapshotData, IntPtr baselineData, ref DataStreamReader reader, ref NetworkCompressionModel compressionModel, IntPtr changeMaskData, int startOffset)
        {
            ref var snapshot = ref GhostComponentSerializer.TypeCast<Snapshot>(snapshotData);
            ref var baseline = ref GhostComponentSerializer.TypeCast<Snapshot>(baselineData);
            uint changeMask = GhostComponentSerializer.CopyFromChangeMask(changeMaskData, startOffset, ChangeMaskBits);
            if ((changeMask & (1 << 0)) != 0)
                snapshot.playerVelocity_x = reader.ReadPackedIntDelta(baseline.playerVelocity_x, compressionModel);
            else
                snapshot.playerVelocity_x = baseline.playerVelocity_x;
            if ((changeMask & (1 << 0)) != 0)
                snapshot.playerVelocity_y = reader.ReadPackedIntDelta(baseline.playerVelocity_y, compressionModel);
            else
                snapshot.playerVelocity_y = baseline.playerVelocity_y;
            if ((changeMask & (1 << 0)) != 0)
                snapshot.playerVelocity_z = reader.ReadPackedIntDelta(baseline.playerVelocity_z, compressionModel);
            else
                snapshot.playerVelocity_z = baseline.playerVelocity_z;
            if ((changeMask & (1 << 1)) != 0)
                snapshot.worldVelocity_x = reader.ReadPackedIntDelta(baseline.worldVelocity_x, compressionModel);
            else
                snapshot.worldVelocity_x = baseline.worldVelocity_x;
            if ((changeMask & (1 << 1)) != 0)
                snapshot.worldVelocity_y = reader.ReadPackedIntDelta(baseline.worldVelocity_y, compressionModel);
            else
                snapshot.worldVelocity_y = baseline.worldVelocity_y;
            if ((changeMask & (1 << 1)) != 0)
                snapshot.worldVelocity_z = reader.ReadPackedIntDelta(baseline.worldVelocity_z, compressionModel);
            else
                snapshot.worldVelocity_z = baseline.worldVelocity_z;
        }
        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        [BurstCompile]
        [MonoPInvokeCallback(typeof(GhostComponentSerializer.ReportPredictionErrorsDelegate))]
        private static void ReportPredictionErrors(IntPtr componentData, IntPtr backupData, ref UnsafeList<float> errors)
        {
            ref var component = ref GhostComponentSerializer.TypeCast<PropHunt.Mixed.Components.KCCVelocity>(componentData, 0);
            ref var backup = ref GhostComponentSerializer.TypeCast<PropHunt.Mixed.Components.KCCVelocity>(backupData, 0);
            int errorIndex = 0;
            errors[errorIndex] = math.max(errors[errorIndex], math.distance(component.playerVelocity, backup.playerVelocity));
            ++errorIndex;
            errors[errorIndex] = math.max(errors[errorIndex], math.distance(component.worldVelocity, backup.worldVelocity));
            ++errorIndex;
        }
        private static int GetPredictionErrorNames(ref FixedString512 names)
        {
            int nameCount = 0;
            if (nameCount != 0)
                names.Append(new FixedString32(","));
            names.Append(new FixedString64("playerVelocity"));
            ++nameCount;
            if (nameCount != 0)
                names.Append(new FixedString32(","));
            names.Append(new FixedString64("worldVelocity"));
            ++nameCount;
            return nameCount;
        }
        #endif
    }
}