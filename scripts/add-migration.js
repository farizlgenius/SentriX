const { execSync } = require("child_process");
const modules = require("./modules");

const moduleName = process.argv[2];
const migrationName = process.argv[3];

if (!moduleName || !migrationName) {
  console.log(
    "Usage: npm run migadd -- <module> <migrationName>"
  );
  process.exit(1);
}

const module = modules.find(
  m => m.name === moduleName.toLowerCase()
);

if (!module) {
  console.error(`Module ${moduleName} not found`);
  process.exit(1);
}

execSync(
  `dotnet ef migrations add ${migrationName} --project ${module.project} --startup-project src/Host`,
  { stdio: "inherit" }
);