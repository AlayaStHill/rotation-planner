import type { Metadata } from "next";
import "./globals.css";

export const metadata: Metadata = {
  title: "RotationPlanner",
  description: "Compare and match rotation schedules",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body>{children}</body>
    </html>
  );
}
