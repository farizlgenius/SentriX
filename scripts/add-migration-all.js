const { execSync } = require("child_process");

const migrationName = process.argv[2];

if (!migrationName) {
    console.error("Migration name is required.");
    process.exit(1);
}

const modules = [
    "src/Modules/Auth/Auth.Infrastructure",
    // "src/Modules/Cache/Cache.Infrastructure",
    "src/Modules/Core/Core.Infrastructure",
    "src/Modules/Location/Location.Infrastructure",
    "src/Modules/Role/Role.Infrastructure",
    "src/Modules/Operator/Operator.Infrastructure",
    "src/Modules/Device/Device.Infrastructure",
    "src/Modules/Adapters/Adapter.Aero",
    "src/Modules/Events/Events.Infrastructure",
    "src/Modules/Output/Output.Infrastructure",
    "src/Modules/Time/Time.Infrastructure",
    "src/Modules/Door/Door.Infrastructure",
    "src/Modules/Group/Group.Infrastructure"
];

for (const project of modules) {
    console.log(`\n=== Adding migration to ${project} ===`);

    try {
        execSync(
            `dotnet ef migrations add ${migrationName} --project ${project} --startup-project src/Host`,
            { stdio: "inherit" }
        );
    } catch (error) {
        console.error(`Failed for ${project}`);
    }
}