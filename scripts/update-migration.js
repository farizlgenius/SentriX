import { execSync } from "child_process";
import modules from "./modules.js"; // Note: Add the .js extension for ESM imports

const moduleName = process.argv[2];

if (!moduleName) {
  console.log("Usage: npm run migadd -- <module>");
  process.exit(1);
}

const targetModule = modules.find((m) => m.name === moduleName.toLowerCase());

if (!targetModule) {
  console.error(`Module ${moduleName} not found`);
  process.exit(1);
}

execSync(
  `dotnet ef database update --project ${targetModule.project} --startup-project src/Host`,
  { stdio: "inherit" },
);
