mkdir APKs

msbuild /t:PackageForAndroid /p:Configuration=Release DecodeConfigSampleAPI/DecodeConfigSampleAPI/DecodeConfigSampleAPI.csproj
msbuild /t:SignAndroidPackage /p:Configuration=Release DecodeConfigSampleAPI/DecodeConfigSampleAPI/DecodeConfigSampleAPI.csproj
Copy-Item ./DecodeConfigSampleAPI/DecodeConfigSampleAPI/bin/Release/DecodeConfigSampleAPI.DecodeConfigSampleAPI-Signed.apk APKs/DecodeConfigSampleAPI.apk

msbuild /t:PackageForAndroid /p:Configuration=Release DecodeIntent/DecodeIntent/DecodeIntent.csproj
msbuild /t:SignAndroidPackage /p:Configuration=Release DecodeIntent/DecodeIntent/DecodeIntent.csproj
Copy-Item ./DecodeIntent/DecodeIntent/bin/Release/DecodeIntent.DecodeIntent-Signed.apk APKs/DecodeIntent.apk

msbuild /t:PackageForAndroid /p:Configuration=Release DecodeListener/DecodeListener/DecodeListener.csproj
msbuild /t:SignAndroidPackage /p:Configuration=Release DecodeListener/DecodeListener/DecodeListener.csproj
Copy-Item ./DecodeListener/DecodeListener/bin/Release/DecodeListener.DecodeListener-Signed.apk APKs/DecodeListener.apk

msbuild /t:PackageForAndroid /p:Configuration=Release DeviceSampleAPI/DeviceSampleAPI/DeviceSampleAPI.csproj
msbuild /t:SignAndroidPackage /p:Configuration=Release DeviceSampleAPI/DeviceSampleAPI/DeviceSampleAPI.csproj
Copy-Item ./DeviceSampleAPI/DeviceSampleAPI/bin/Release/DeviceSampleAPI.DeviceSampleAPI-Signed.apk APKs/DeviceSampleAPI.apk

msbuild /t:PackageForAndroid /p:Configuration=Release JoyaTouchCradleSampleAPI/JoyaTouchCradleSampleAPI/JoyaTouchCradleSampleAPI.csproj
msbuild /t:SignAndroidPackage /p:Configuration=Release JoyaTouchCradleSampleAPI/JoyaTouchCradleSampleAPI/JoyaTouchCradleSampleAPI.csproj
Copy-Item ./JoyaTouchCradleSampleAPI/JoyaTouchCradleSampleAPI/bin/Release/JoyaTouchCradleSampleAPI.JoyaTouchCradleSampleAPI-Signed.apk APKs/JoyaTouchCradleSampleAPI.apk
