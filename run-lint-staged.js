const { execSync } = require("child_process");
const path = require("path");

const frontendDir = path.join(__dirname, "CleanFix-frontend");

try {
  execSync("npm run lint-staged", {
    cwd: frontendDir,
    stdio: "inherit",
    shell: true,
  });
} catch (err) {
  process.exit(1);
}
