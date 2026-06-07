const { execSync } = require("child_process");
const modules = require("./modules");

for (const module of modules) {
  console.log(`\nUpdating ${module.name}`);

  execSync(
    `dotnet ef database update --project ${module.project} --startup-project src/Host`,
    { stdio: "inherit" }
  );
}