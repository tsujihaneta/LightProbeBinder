# LightProbeBinder
[【Unity】Timelineとマルチシーンでライトベイク済みの背景モデルを切り替えるエディタ拡張『LightProbe Binder』を公開しました](https://qiita.com/drafts/084b0403839067d67a86/edit)

An editor extension that restores light probes when the specified game object is loaded; Multi-Scene and Timeline support are also available.

**Before(Light probes from the last scene added will be used)**

![Before](https://user-images.githubusercontent.com/26054187/156917794-5ade96dc-a1a8-4ab8-b09a-deccdec9e295.gif)

**After(Appropriate light probes will be used)**

![After](https://user-images.githubusercontent.com/26054187/156917802-cc4a220e-f8f2-472d-bcec-4f02d0daae9b.gif)

**Example**

![ezgif-4-86a13dc318](https://user-images.githubusercontent.com/26054187/156904064-8dc1c401-51a1-47fa-af48-80ee55b63479.gif)

![image](https://user-images.githubusercontent.com/26054187/156904094-5070f03b-c54a-44a8-86bf-e61127bd2d43.png)
![image](https://user-images.githubusercontent.com/26054187/156904125-ac648e4c-3614-492b-9796-2596fd1cb982.png)

使用方法
1. LightProbe Binderをプロジェクトに追加します。（Examplesフォルダは削除しても構いません。）
2. 加算シーンとして使用する背景のみのシーン（以下「背景シーン」）を作成します。
![68747470733a2f2f71696974612d696d6167652d73746f72652e73332e61702d6e6f727468656173742d312e616d617a6f6e6177732e636f6d2f302f3237323037362f31313035376238372d336561302d653861312d643734302d3836313563663165353261352e706e67](https://user-images.githubusercontent.com/26054187/157849990-0d8dfab8-9aec-47fc-8f09-7b1244cda4cf.png)
3. staticで背景に使用するすべてのオブジェクトを一つのGameObjectの子オブジェクトにします。
![68747470733a2f2f71696974612d696d6167652d73746f72652e73332e61702d6e6f727468656173742d312e616d617a6f6e6177732e636f6d2f302f3237323037362f35376436653766382d323161662d633563342d343130632d3162356338396134333162322e706e67](https://user-images.githubusercontent.com/26054187/157850090-2817be93-2365-4ff9-9d77-9c42d10fec43.png)
4. 「背景シーン」を単体で開いた状態で、通常通りLightingウィンドウの「Generate Lighting」でライトをベイクします。

6. staticで背景に使用するすべてのオブジェクトを一つのGameObjectの子オブジェクトにします。
7. 加算シーンとして使用する背景のみのシーンを単体で開き、通常通りLightingウィンドウの「Generate Lighting」でライトをベイクします。
8. Tool→LightProbeBinderを開き、Owner Objectに2で作成した背景オブジェクトのRootオブジェクトを指定します。
9. 「Store Prebe」を押します。
10. シーンのLightingDataからLightProbeのAssetがシーンと同名のフォルダ（ライトマップ出力と同じフォルダ）にエクスポートされ、RootオブジェクトにLightProbeRestorerコンポーネントがアタッチされます。
11. 以降、このオブジェクトが有効化された際自動でLightProbeが読み込まれるようになります。

How to use
1. Creates a background-only scene to be used as an additive scene.
2. Set all objects used for backgrounds at rest to be children of a single GameObject.
3. Open a stand-alone background-only scene to be used as an additive scene and bake the lights as usual with "Generate Lighting" in the Lighting window.
4. Open Tool/LightProbeBinder and specify the Root object of the background object created in 2 in Owner Object.
5. Press "Store Prebe".
6. A LightProbe Asset is exported from the scene's LightingData to a folder with the same name as the scene (the same folder as the lightmap output), and a LightProbeRestorer component is attached to the Root object.
7. After that, LightProbe will be automatically loaded when the owner object is activated.
