const { execSync } = require("child_process");

// const modules = [
//     "src/Modules/Auth/Auth.Infrastructure",
//     "src/Modules/User/User.Infrastructure",
//     "src/Modules/Location/Location.Infrastructure",
//     "src/Modules/Role/Role.Infrastructure",
//     "src/Modules/Operator/Operator.Infrastructure",
//     "src/Modules/Device/Device.Infrastructure",
//     "src/Modules/Adapters/Adapter.Aero",
//     "src/Modules/Events/Events.Infrastructure",
//     "src/Modules/Output/Output.Infrastructure",
//     "src/Modules/Time/Time.Infrastructure",
//     "src/Modules/Door/Door.Infrastructure",
//     "src/Modules/Group/Group.Infrastructure",
//     "src/Modules/Input/Input.Infrastructure",
//     "src/Modules/Setting/Setting.Infrastructure",
// ];

const modules = [
  "src/Modules/Auth/Auth.Infrastructure",
  "src/Modules/Core/Core.Infrastructure",
  "src/Modules/Adapters/Adapter.Aero",
  "src/Modules/Setting/Setting.Infrastructure",
];

for (const project of modules) {
  console.log(`\n=== Adding migration to ${project} ===`);

  try {
    execSync(
      `dotnet ef database update --project ${project} --startup-project src/Host`,
      { stdio: "inherit" },
    );
  } catch (error) {
    console.error(`Failed for ${project}`);
  }
}
