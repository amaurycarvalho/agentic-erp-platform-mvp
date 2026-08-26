import glob
import sys
import xml.etree.ElementTree as ET

threshold = float(sys.argv[1]) if len(sys.argv) > 1 else 80.0
results = {}
for name in ("Agent", "Mcp", "ErpAcl", "Rag"):
    files = glob.glob(f"TestResults/{name}/**/coverage.cobertura.xml", recursive=True)
    covered = valid = 0
    for f in files:
        try:
            root = ET.parse(f).getroot()
            covered += float(root.get("lines-covered") or 0)
            valid += float(root.get("lines-valid") or 0)
        except Exception:
            pass
    pct = (covered / valid * 100) if valid else 0
    results[name] = pct
    print(f"  {name}: {pct:.1f}%  ({int(covered)}/{int(valid)})")

failed = [n for n, p in results.items() if p < threshold or p == 0]
if failed:
    print(f"ERROR: coverage below threshold ({threshold}%): {', '.join(failed)}")
    sys.exit(1)
print(f"OK: coverage above threshold ({threshold}%)")
