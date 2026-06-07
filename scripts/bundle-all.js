const { execSync } = require("child_process");
const modules = require("./modules");

for (const module of modules) {
  console.log(`Creating bundle for ${module.name}`);

  execSync(
    `dotnet ef migrations bundle \
      --project ${module.project} \
      --startup-project src/Host \
      -o deploy/${module.name}-migration.exe`,
    { stdio: "inherit" }
  );
}