const fs = require("fs");
const path = require("path");

const rootDir = path.resolve(__dirname, "..");

function deleteMigrationsFolders(dir) {
  const entries = fs.readdirSync(dir, { withFileTypes: true });

  for (const entry of entries) {
    const fullPath = path.join(dir, entry.name);

    if (entry.isDirectory()) {
      if (entry.name === "Migrations") {
        console.log(`Deleting: ${fullPath}`);
        fs.rmSync(fullPath, {
          recursive: true,
          force: true,
        });
        continue;
      }

      deleteMigrationsFolders(fullPath);
    }
  }
}

try {
  deleteMigrationsFolders(rootDir);
  console.log("✅ All Migrations folders deleted.");
} catch (err) {
  console.error("❌ Error deleting migrations:", err);
  process.exit(1);
}