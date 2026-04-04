import * as dateFns from 'date-fns';
import fs from 'fs';
import { execSync } from 'child_process';

(async () => {
  const latestTag = execSync('git describe --tags --abbrev=0')
    .toString()
    .trim();

  const commits = execSync(`git log --oneline --no-merges ${latestTag}..HEAD`)
    .toString()
    .trim()
    .split('\n')
    .filter(Boolean);

  const countCommits = (type: string) => commits.filter((c) => c.includes(`${type}:`)).length;

  const numberOfFeatureCommits = countCommits('feat');
  const numberOfChoreCommits = countCommits('chore');
  const numberOfFilesChanged = parseInt(
    execSync(`git diff --name-only ${latestTag}..HEAD | wc -l`)
      .toString()
      .trim(),
    10,
  );

  console.log('Commits:', commits);
  console.log('Latest tag:', latestTag);
  console.log('Files changed:', numberOfFilesChanged);

  const pkg = JSON.parse(fs.readFileSync('./package.json', 'utf-8'));
  const pkgLock = JSON.parse(fs.readFileSync('./package-lock.json', 'utf-8'));
  const [major, minor, patch] = pkg.version.split('.').map(Number);

  let newMajor = major;
  let newMinor = minor;
  let newPatch = patch;

  if (numberOfFeatureCommits > 0 || numberOfFilesChanged >= 25) {
    newMajor++;
    newMinor = 0;
    newPatch = 0;
  }
  else if (numberOfChoreCommits > 0) {
    newMinor++;
    newPatch = 0;
  }
  else {
    newPatch++;
  }

  const newVersion = `${newMajor}.${newMinor}.${newPatch}`;
  console.log('New version:', newVersion);

  pkg.version = newVersion;
  pkgLock.version = newVersion;
  fs.writeFileSync('./package.json', JSON.stringify(pkg, null, 2));
  fs.writeFileSync('./package-lock.json', JSON.stringify(pkgLock, null, 2));

  ['frontend', 'backend'].forEach(folder => {
    const p = `${folder}/package.json`;
    const subpkg = JSON.parse(fs.readFileSync(p, 'utf8'));
    subpkg.version = newVersion;
    fs.writeFileSync(p, JSON.stringify(subpkg, null, 2));
  });

  const now = dateFns.format(new Date(), 'dd MMM yyyy HH:mm:ssa')
    .replace('PM', 'pm')
    .replace('AM', 'am');
  const entry = `## Version ${newVersion}\nReleased **${now}** - *${commits.length} commits*\n- ${commits.join('\n- ')}`;
  const changelogFilePath = './CHANGELOG.md';
  const existingChangelog = fs.readFileSync(changelogFilePath).toString();
  const newChangelog = `${entry}\n\n${existingChangelog} `;
  fs.writeFileSync(changelogFilePath, newChangelog);

  execSync('git add .');
  execSync(`git commit -m 'release: version ${newVersion}'`);
  execSync('git push');
  execSync(`git tag ${newVersion}`);
  execSync('git push --tags');
})();