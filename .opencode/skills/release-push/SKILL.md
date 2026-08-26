---
name: release-push
description: Push the current release to GitHub by creating a tag and release branch. Use after release-version to publish the release.
license: MIT
metadata:
  author: amaurycarvalho
  version: "1.2"
---

Push the current release to the remote repository: create a git tag, push tags, create a release branch, push it, and switch back to `main`.

The version is read automatically from `VERSION` in the `Makefile` (the single source of truth for image and release version, e.g. `VERSION ?= 1.0.0`).

**Steps**

1. **Verify if release's changes are all archived**

   Confirm if all of the changes belonging to the release being pushed are archived. Only the changes of the version being pushed are checked — active changes under `## [Unreleased]` do NOT block the release.

   a. **Read the release version** from `VERSION` in the `Makefile` (e.g. `1.0.1`).

   b. **Identify the release's changes** from `CHANGELOG.md`: find the `## [<version>] -` heading and collect every `### [<change-name>](...)` entry under it (until the next `## [` heading or EOF). Entries under `## [Unreleased]` do NOT belong to this release and must be ignored. If no `## [<version>]` heading exists, report an error and STOP.

   c. **Confirm each release change is archived**: a directory named `<change-name>` exists under `openspec/changes/archive/` (e.g. `openspec/changes/archive/YYYY-MM-DD-<change-name>`).

   d. Stop and report the error without proceeding if any of the release's changes is not archived yet.

**Output On Success**

```
Checking if all release's changes are archived: OK.
```

**Output On Error**

```
Checking if all release's changes are archived: Fail.
ERROR: Change <change-name> not archived yet.
```

2. **Run the release script**

   Execute the bash script located at `.opencode/skills/release-push/release-push.sh`. This script:
   - Reads the version from `VERSION` in the `Makefile`
   - Creates the git tag `v<version>`
   - Pushes tags to origin
   - Creates and pushes a `release/v<version>` branch
   - Switches back to `main`

   Use the `bash` tool to run the script.

3. **Verify**

   Confirm the script completed successfully (exit code 0). If it failed, report the error and STOP.

**Output On Success**

```
=== Release v<version> pushed to GitHub successfully ===
```

**Output On Error**

```
ERROR: Could not extract version from Makefile.
```

**Guardrails**
- Run the script — do NOT execute the git commands manually.
- Do NOT modify any files.
- If the script fails, stop and report the error without proceeding.
