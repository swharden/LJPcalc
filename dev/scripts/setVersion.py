import os
import sys

PATH_HERE = os.path.dirname(__file__)
PATH_SRC = PATH_HERE+"../../src/"
csprojPaths = [
    PATH_SRC+"LJPmath/LJPmath.csproj",
    PATH_SRC+"LJPcalc/LJPcalc.csproj",
    PATH_SRC+"LJPconsole/LJPconsole.csproj"
]
csprojPaths = [os.path.abspath(x) for x in csprojPaths]
for path in csprojPaths:
    if not os.path.exists(path):
        raise Exception("csproj file not found: " + path)

def setVersion(filePath, newVersion, tag):

    with open(filePath) as f:
        lines = f.read().split("\n")

    for i, line in enumerate(lines):
        if tag in line:
            linePre = line.split(">")[0] + ">"
            linePost = "<" + line.split("<")[2]
            lineNew = linePre + newVersion + linePost
            lines[i] = lineNew

    with open(filePath, 'w') as f:
        f.write("\n".join(lines))

    basename = os.path.basename(filePath)
    print(f"set {basename} {tag} to {newVersion}")

if __name__=="__main__":
    if len(sys.argv) == 2:
        newVersion = sys.argv[1].strip()
        for filePath in csprojPaths:
            setVersion(filePath, newVersion, "<Version>")
            setVersion(filePath, newVersion, "<AssemblyVersion>")
            setVersion(filePath, newVersion, "<FileVersion>")
            print()
    else:
        print("ERROR: version argument required")