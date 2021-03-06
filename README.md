# LightProbeBinder
[【Unity】Timelineとマルチシーンでライトベイク済みの背景モデルを切り替えるエディタ拡張『LightProbe Binder』を公開しました](https://qiita.com/drafts/084b0403839067d67a86/edit)

An editor extension that restores light probes when the specified game object is loaded; Multi-Scene and Timeline support are also available.

**Before(Light probes from the last scene added will be used)**

![Before](https://user-images.githubusercontent.com/26054187/156917794-5ade96dc-a1a8-4ab8-b09a-deccdec9e295.gif)

**After(Appropriate light probes will be used)**

![After](https://user-images.githubusercontent.com/26054187/156917802-cc4a220e-f8f2-472d-bcec-4f02d0daae9b.gif)

**Example**

![ezgif-4-86a13dc318](https://user-images.githubusercontent.com/26054187/156904064-8dc1c401-51a1-47fa-af48-80ee55b63479.gif)

使用方法
How to use

1. LightProbe Binderをプロジェクトに追加します。（Examplesフォルダは削除しても構いません。）
　Add LightProbe Binder to your project. (You may delete the Examples folder if you wish.)

2. 加算シーンとして使用する背景のみのシーン（以下「背景シーン」）を作成します。
　Create a background-only scene ("background scene") to be used as an additive scene.

![68747470733a2f2f71696974612d696d6167652d73746f72652e73332e61702d6e6f727468656173742d312e616d617a6f6e6177732e636f6d2f302f3237323037362f31313035376238372d336561302d653861312d643734302d3836313563663165353261352e706e67](https://user-images.githubusercontent.com/26054187/157849990-0d8dfab8-9aec-47fc-8f09-7b1244cda4cf.png)

3. staticで背景に使用するすべてのオブジェクトを一つのGameObjectの子オブジェクトにします。
　static and make all objects used for backgrounds child objects of a single GameObject.

![68747470733a2f2f71696974612d696d6167652d73746f72652e73332e61702d6e6f727468656173742d312e616d617a6f6e6177732e636f6d2f302f3237323037362f35376436653766382d323161662d633563342d343130632d3162356338396134333162322e706e67](https://user-images.githubusercontent.com/26054187/157850090-2817be93-2365-4ff9-9d77-9c42d10fec43.png)

4. 「背景シーン」を単体で開いた状態で、通常通りLightingウィンドウの「Generate Lighting」でライトをベイクします。
With the "background scene" open by itself, bake the lights as usual with "Generate Lighting" in the Lighting window.

![68747470733a2f2f71696974612d696d6167652d73746f72652e73332e61702d6e6f727468656173742d312e616d617a6f6e6177732e636f6d2f302f3237323037362f32613330333563362d303061342d313264612d636133312d3762313664383865393430642e706e67](https://user-images.githubusercontent.com/26054187/157850453-a7b0498a-5577-4cfd-a747-745f3d54f987.png)
![68747470733a2f2f71696974612d696d6167652d73746f72652e73332e61702d6e6f727468656173742d312e616d617a6f6e6177732e636f6d2f302f3237323037362f33376262353666372d636163342d636162662d306535392d6433653933346661623035352e706e67](https://user-images.githubusercontent.com/26054187/157850529-e33c109a-d8cc-4b6b-8037-025fc6705c2d.png)

5. 「Tools」→「Light Probe Binder」を開きます。
 Open "Tools" -> "Light Probe Binder".
 
![68747470733a2f2f71696974612d696d6167652d73746f72652e73332e61702d6e6f727468656173742d312e616d617a6f6e6177732e636f6d2f302f3237323037362f35373138633065352d363766332d313761332d393030612d3239396339633662623765632e706e67](https://user-images.githubusercontent.com/26054187/157850636-f1ded61c-4704-4715-9dea-278ed11ffefb.png)

![68747470733a2f2f71696974612d696d6167652d73746f72652e73332e61702d6e6f727468656173742d312e616d617a6f6e6177732e636f6d2f302f3237323037362f39383739323063392d366131362d393564362d653730612d3733373664653435333064612e706e67](https://user-images.githubusercontent.com/26054187/157850651-96a5fcde-6d6b-44d7-a9d8-11dd1f98ec4d.png)

6. 「Binding Settings」の「Root Object」に2で作成した背景オブジェクトのRootオブジェクトを指定し「Subscribe」ボタンを押します。
　Specify the Root object of the background object created in step 2 in the "Root Object" field of "Binding Settings" and click the "Subscribe" button.

![68747470733a2f2f71696974612d696d6167652d73746f72652e73332e61702d6e6f727468656173742d312e616d617a6f6e6177732e636f6d2f302f3237323037362f63303934313364622d303434362d323138372d656665622d6662326633616438373766652e706e67](https://user-images.githubusercontent.com/26054187/157850765-30d47c3e-8aeb-4f4d-8c66-40ea1863f29c.png)

7. LightProbesAssetが生成され、「Owner Management」に先程のオブジェクトが登録されます。
　A LightProbesAsset is generated and the object is registered in "Owner Management".

![68747470733a2f2f71696974612d696d6167652d73746f72652e73332e61702d6e6f727468656173742d312e616d617a6f6e6177732e636f6d2f302f3237323037362f64626232653139612d656635392d376438322d616237662d3735336537656166656637612e706e67](https://user-images.githubusercontent.com/26054187/157850828-9135677b-dac8-43bf-8793-596e370f4467.png)

![68747470733a2f2f71696974612d696d6167652d73746f72652e73332e61702d6e6f727468656173742d312e616d617a6f6e6177732e636f6d2f302f3237323037362f66383766303536642d306164612d373632352d623430622d6566303938633564653635362e706e67](https://user-images.githubusercontent.com/26054187/157850849-a78bfbe8-82a5-4cd0-8b62-0967a7578674.png)

※Ownerになったオブジェクトには「LightProbeRestorer」コンポーネントがアタッチされます。
　The "LightProbeRestorer" component is attached to the object that becomes the Owner.
 
![68747470733a2f2f71696974612d696d6167652d73746f72652e73332e61702d6e6f727468656173742d312e616d617a6f6e6177732e636f6d2f302f3237323037362f66363636393130662d663562382d336330322d333230322d6235366166346462303862662e706e67](https://user-images.githubusercontent.com/26054187/157850886-840636a5-8675-48a0-82b8-5394e260eae2.png)

8. これで設定は完了です。以降、このオブジェクトが有効化された際自動でLightProbeが読み込まれるようになります。
　This completes the setup. From now on, LightProbe will be automatically loaded when this object is activated.

![68747470733a2f2f71696974612d696d6167652d73746f72652e73332e61702d6e6f727468656173742d312e616d617a6f6e6177732e636f6d2f302f3237323037362f30383563313033372d366636632d346436622d333466372d3036613061653635653430362e676966](https://user-images.githubusercontent.com/26054187/157850943-00b5b5b1-7bac-4a5e-9ee1-e1f2f4c5c522.gif)
