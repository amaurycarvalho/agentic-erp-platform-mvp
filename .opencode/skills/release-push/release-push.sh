#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
PROJECT_ROOT="$(cd "$SCRIPT_DIR/../../.." && pwd)"
MAKEFILE="$PROJECT_ROOT/Makefile"

if [ ! -f "$MAKEFILE" ]; then
    echo "ERROR: Could not find $MAKEFILE" >&2
    exit 1
fi

VERSION=$(sed -n 's/^VERSION ?= *\(.*\)/\1/p' "$MAKEFILE" | head -n 1)

if [ -z "$VERSION" ]; then
    echo "ERROR: Could not extract version from $MAKEFILE" >&2
    exit 1
fi

echo "=== Releasing v$VERSION ==="

if git rev-parse -q --verify "refs/tags/v$VERSION" > /dev/null; then
    echo "ERROR: Tag v$VERSION already exists" >&2
    exit 1
fi

if git show-ref -q --verify "refs/heads/release/v$VERSION"; then
    echo "ERROR: Branch release/v$VERSION already exists" >&2
    exit 1
fi

git tag -a "v$VERSION" -m "Release $VERSION"
echo "  ✓ Tag v$VERSION created"

git push origin --tags
echo "  ✓ Tags pushed"

git checkout -b "release/v$VERSION"
echo "  ✓ Branch release/v$VERSION created"

git push origin "release/v$VERSION"
echo "  ✓ Branch release/v$VERSION pushed"

git switch main
echo "  ✓ Switched back to main"

echo ""
echo "=== Release v$VERSION pushed to GitHub successfully ==="
