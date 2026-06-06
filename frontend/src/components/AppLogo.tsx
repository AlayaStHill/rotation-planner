import Image from "next/image";

export function AppLogo() {
  return (
    <Image
      src="/images/logo-transparent.webp"
      alt="RotationPlanner logo"
      width={420}
      height={105}
      priority
    />
  );
}
