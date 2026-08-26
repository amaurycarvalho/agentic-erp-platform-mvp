---
name: release-push
description: Push the current release to GitHub by creating a tag and release branch. Use after release-version to publish the release.
license: MIT
metadata:
  author: amaurycarvalho
  version: "1.1"
---

Push the current release to the remote repository: create a git tag, push tags, create a release branch, push it, and switch back to `main`.

The version is read automatically from `VERSION` in the `Makefile` (the single source of truth for image and release version, e.g. `VERSION ?= 1.0.0`).

**Steps**

1. **Verify if release's changes are all archived**

   Confirm if all of the changes in this release are archived. The release consists of the active OpenSpec changes — the directories directly under `openspec/changes/` (excluding `archive`). A change is archived when it was moved to `openspec/changes/archive/` (e.g. `YYYY-MM-DD-<change-name>`). Stop and report the error without proceeding if any of them is not archived yet.

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
