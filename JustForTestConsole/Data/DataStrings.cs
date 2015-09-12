namespace JustForTestConsole.Data
{
    public static class DataStrings
    {
        public const string CULTURE_MARK = "*culture*";
        public static string activity = "com.qvc.EULA.EulaPage";

        public static string path = "d:\\";
        public static string pShellDownload = "powershell -command \"& { (New-Object Net.WebClient).DownloadFile('";
        public static string adbInstall = "adb -s *deviceId* install ";
        public static string adbUninstall = "adb -s *deviceId* uninstall ";
        public static string adbPackages = "adb -s *deviceId* shell \"pm list packages\"";
        public static string adbPackagesGrep = "adb -s *deviceId* shell \"pm list packages | grep *filter*\"";
        public static string adbDevices = "adb devices";
        public static string adbClear = "adb -s *deviceId* shell pm clear ";
        public static string adbForceStop = "adb -s *deviceId* shell am force-stop ";
        public static string adbStartApp = "";

        public static string adbModel = "adb -s *deviceId* shell getprop ro.product.model";

        
        public static string linkQa = "https://dl.dropbox.com/u/25719532/apps/android_*culture*_tabletopt_qa/QVC_*culture*_TabletOpt_QA.apk";

        public static string linkStage = "https://dl.dropbox.com/u/25719532/apps/android_*culture*_tabletopt_stage/QVC_*culture*_TabletOpt_Stage.apk";

        public static string linkCiQa = "https://dl.dropbox.com/u/25719532/apps/android_*culture*_fragment_ci_qa/QVC_*culture*_TabletOpt_CI_QA.apk";

        public static string linkCiStage =
            "https://dl.dropbox.com/u/25719532/apps/android_*culture*_fragment_ci_stage/QVC_*culture*_TabletOpt_CI_Stage.apk";

        //Websites
        public static string webAdress = "https://dl.dropboxusercontent.com/u/25719532/apps/android_*culture*_*kind*_*type*/index.html";

        public static string webQa = "https://dl.dropboxusercontent.com/u/25719532/apps/android_*culture*_tabletopt_qa/index.html";

        public static string webStage = "https://dl.dropboxusercontent.com/u/25719532/apps/android_*culture*_tabletopt_stage/index.html";

        public static string webCiQa = "https://dl.dropboxusercontent.com/u/25719532/apps/android_*culture*_fragment_ci_qa/index.html";

        public static string webCiStage =
            "https://dl.dropboxusercontent.com/u/25719532/apps/android_*culture*_fragment_ci_stage/index.html";
        
        //Apk's
        public static string QaApk = "QVC_*culture*_TabletOpt_QA.apk";
        public static string StageApk = "QVC_*culture*_TabletOpt_Stage.apk";
        public static string QaCiApk = "QVC_*culture*_TabletOpt_CI_QA.apk";
        public static string StageCiApk = "QVC_*culture*_TabletOpt_CI_Stage.apk";

        //Package names
        public static string QaPackageName = "com.qvc.tabletopt.qa";
        public static string StagePackageName = "com.qvc.tabletopt.stage";
        public static string QaCiPackageName = "com.qvc.fragment.ci.qa";
        public static string StageCiPackageName = "com.qvc.fragment.ci.stage";


        //unsort
        public static string openApp = "adb -s *deviceId* shell am start -n *package*/" + activity;
        //public static string openApp = "adb -s *deviceId* shell am start -n com.qvc.fragment.ci.stage/com.qvc.EULA.EulaPage";

        //Get version of installed
        //adb shell "dumpsys package com.qvc.fragment.ci.stage | grep versionName"

        //How to recognize culture by .apk file on PC: using aapt tool
        //aapt d badging d:\QVC.apk
        //
        //us:  package: name='com.qvc.tabletopt.qa'
        //uk:  package: name='com.qvcuk.tabletopt.qa'
        //de:  package: name='de.qvc.tabletopt.qa'
        //
        //How to recognize culture by build installed on device: 
        //adb -s *deviceId* shell "pm list packages | grep *filter*"
        //filter should one of the next: 
        //us: com.qvc. 
        //uk: com.qvcuk.
        //de: de.qvc.
    }
}
