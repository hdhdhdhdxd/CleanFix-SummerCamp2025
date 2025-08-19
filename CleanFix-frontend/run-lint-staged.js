const { execSync } = require('child_process')
const path = require('path')

const frontendDir = path.join(__dirname)

try {
  execSync('npm run lint-staged --prefix "' + frontendDir + '"', {
    stdio: 'inherit',
    shell: true,
  })
} catch (err) {
  process.exit(1)
}
