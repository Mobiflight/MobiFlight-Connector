import DeviceSummaryCard from "@/components/mobiflight/DeviceSummaryCard";
import { useDevicesStore } from "@/stores/deviceStateStore";

export default function DevicesPage() {
  const { devices } = useDevicesStore();
  return (
    <div className="flex flex-col overflow-y-auto gap-4">
      <div className="flex flex-row gap-4 items-center">
        <p className="scroll-m-20 text-3xl tracking-tight first:mt-0 font-bold">
          Devices
        </p>
      </div>
      <div className="flex flex-row flex-wrap w-full gap-4 overflow-y-auto">
        {devices?.map((device) => (
          <DeviceSummaryCard className="w-96 h-[]" key={device.Id} device={device}></DeviceSummaryCard>
        ))}
      </div>
    </div>
  );
}
