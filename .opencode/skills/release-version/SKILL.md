---
name: release-version
description: Update the application release version across all version files and regenerate the changelog. Use when the user wants to set a new release version.
license: MIT
metadata:
  author: amaurycarvalho
  version: "2.0"
---

Update the application release version across the codebase and regenerate all changelog-related files.

**Input**: Version string (e.g., `1.0.1`). Must be provided by the user in the command or message. If omitted, show an error message and STOP.

**Steps**

1. **Extract the version**

   Parse the version string from the user's message or command. The version should match the pattern `X.Y.Z` (three numeric segments separated by dots). Examples: `1.0.1`, `2.0.0`.

   **If no version is provided** (the user did not include a version string in their message):
   - Display the following error message and STOP:
     ```
     ERROR: No version provided.
     Usage: /release-version <version>
     Example: /release-version 1.0.1
     ```
   - Do NOT proceed with any of the steps below.

2. **Update VERSION in the Makefile**

   Read `Makefile` and find the line containing `VERSION ?=` (the single source of truth for image and release version).

   Replace the current value with the new version string:

   ```
   VERSION ?= <new-version>
   ```

3. **Verify consistency**

   Read back the modified file and confirm:
   - `Makefile` has `VERSION ?= <new-version>`

**Output On Success**

```
Release version updated to <version>

Files updated:
- Makefile (VERSION)

Commentary: changelog skill can now be used manually to update the changelog files.
```

**Output On Error**

```
ERROR: No version provided.
Usage: /release-version <version>
Example: /release-version 1.0.1
```

**Guardrails**

- Always validate that a version string was provided before making any changes
- Do NOT guess or auto-generate a version — the user must supply it explicitly
- The version format should follow `X.Y.Z` (e.g., `1.0.1`)
- Only update the `VERSION` line in the `Makefile` — service images and the GitHub release version are derived from it at build/release time
