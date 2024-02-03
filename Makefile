DEV_INSTALL_DIR = "C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\BepInEx\plugins\TreeWindController"

build-ui:
	npx esbuild ui/main.jsx --bundle --outfile=dist/bundle.js

build: build-ui
	dotnet build

dev-install: build
	IF not exist $(DEV_INSTALL_DIR) mkdir $(DEV_INSTALL_DIR)
	copy bin\Debug\netstandard2.1\0Harmony.dll $(DEV_INSTALL_DIR)
	copy bin\Debug\netstandard2.1\TreeWindController.dll $(DEV_INSTALL_DIR)

ui-update-live: build-ui
	copy dist/bundle.js "C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\StreamingAssets\~UI~\HookUI\Extensions\panel.tree-wind-controller.js"