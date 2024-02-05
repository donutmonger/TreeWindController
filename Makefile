DLL_OUT_DIR = "bin\Debug\netstandard2.1"
DEV_INSTALL_DIR = "C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\BepInEx\plugins\TreeWindController"
PKG_DIR = "dist\pkg"
THUNDERSTORE_ZIP = "dist\TreeWindController_thunderstore.zip"

all: build

clean-pkg:
	IF exist $(PKG_DIR) rmdir /s /q $(PKG_DIR)

ensure-pkg-dir:
	IF not exist $(PKG_DIR) mkdir $(PKG_DIR)

build-ui:
	npx esbuild frontend/main.jsx --bundle --outfile=dist/bundle.js

build: build-ui
	dotnet build

pkg: ensure-pkg-dir build
	copy $(DLL_OUT_DIR)\0Harmony.dll $(PKG_DIR)
	copy $(DLL_OUT_DIR)\TreeWindController.dll $(PKG_DIR)
	copy frontend\wind.svg $(PKG_DIR)
	copy manifest.json $(PKG_DIR)
	copy icon.png $(PKG_DIR)
	copy README.md $(PKG_DIR)

pkg-thunderstore: clean-pkg pkg
	IF exist $(THUNDERSTORE_ZIP) del $(THUNDERSTORE_ZIP)
	powershell Compress-Archive -Path "$(PKG_DIR)\*" -DestinationPath $(THUNDERSTORE_ZIP)

dev-install: pkg
	IF not exist $(DEV_INSTALL_DIR) mkdir $(DEV_INSTALL_DIR)
	copy $(PKG_DIR)\* $(DEV_INSTALL_DIR)

ui-update-live: build-ui
	copy dist\bundle.js "C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\StreamingAssets\~UI~\HookUI\Extensions\panel.tree-wind-controller.js"
	