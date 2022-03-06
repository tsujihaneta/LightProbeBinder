# LightProbeBinder
[【Unity】Timelineとマルチシーンでライトベイク済みの背景モデルを切り替えるエディタ拡張『LightProbe Binder』を公開しました](https://qiita.com/drafts/084b0403839067d67a86/edit)

An editor extension that restores light probes when the specified game object is loaded; Multi-Scene and Timeline support are also available.

![ezgif-4-86a13dc318](https://user-images.githubusercontent.com/26054187/156904064-8dc1c401-51a1-47fa-af48-80ee55b63479.gif)

![image](https://user-images.githubusercontent.com/26054187/156904094-5070f03b-c54a-44a8-86bf-e61127bd2d43.png)
![image](https://user-images.githubusercontent.com/26054187/156904125-ac648e4c-3614-492b-9796-2596fd1cb982.png)

1. 加算シーンとして使用する背景のみのシーンを作成します。
2. staticで背景に使用するすべてのオブジェクトを一つのGameObjectの子オブジェクトにします。
3. 加算シーンとして使用する背景のみのシーンを単体で開き、通常通りLightingウィンドウの「Generate Lighting」でライトをベイクします。
4. Tool→LightProbeBinderを開き、Owner Objectに2で作成した背景オブジェクトのRootオブジェクトを指定します。
5. 「Store Prebe」を押します。
6. シーンのLightingDataからLightProbeのAssetがシーンと同名のフォルダ（ライトマップ出力と同じフォルダ）にエクスポートされ、RootオブジェクトにLightProbeRestorerコンポーネントがアタッチされます。
7. 以降、このオブジェクトが有効化された際自動でLightProbeが読み込まれるようになります。


1. Creates a background-only scene to be used as an additive scene.
2. Set all objects used for backgrounds at rest to be children of a single GameObject.
3. Open a stand-alone background-only scene to be used as an additive scene and bake the lights as usual with "Generate Lighting" in the Lighting window.
4. Open Tool/LightProbeBinder and specify the Root object of the background object created in 2 in Owner Object.
5. Press "Store Prebe".
6. A LightProbe Asset is exported from the scene's LightingData to a folder with the same name as the scene (the same folder as the lightmap output), and a LightProbeRestorer component is attached to the Root object.
7. After that, LightProbe will be automatically loaded when the owner object is activated.
