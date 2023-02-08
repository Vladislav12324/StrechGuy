using System.Collections;
using System.Collections.Generic;
using Sources.LanguageManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TextTranslate : MonoBehaviour
{
    public string test;
    public TMP_Text textName, Skip, ChooseYorBonus, Fly, DoubleRangeLenght, WithoutBonus,Settings,Close, Skins,Next,IronSkin,InfinityLength,Retry,flyBord,doubleBord,infinityBord,ironBord;
    public Text hi_textName, hi_Skip, hi_ChooseYorBonus, hi_Fly, hi_DoubleRangeLenght, hi_WithoutBonus, hi_Settings, hi_Close, hi_Skins, hi_Next, hi_IronSkin, hi_InfinityLength,hi_Retry;
    public LangExtern script;
    public string[] lang = { "eng", "ru", "pt", "es", "ar", "hi", "tr", "ja", "fr", "id", "de", "it", "zh" };
    private string[] level_name = { "Level", "Уровень", "Nível", "Nivel", "المستوى", "लेवल", "Seviye", "レベル", "Niveau", "Level", "Level", "Livello", "关卡" };
    private string[] skip_name = { "Skip", "Пропустить", "Pular", "Saltar", "تخطي", "छोड़ें", "Atla", "スキップ", "Passer", "Lewati", "Überspringen", "Salta", "跳过" };
    private string[] chouse_bonus_name = { "Choose Yor Bonus", "Выбери Бонус", "Escolhe o teu bónus", "Elige tu bono", " اختر مكافأتك", "अपना बोनस चुनें", "Bonusunu Seç", "ボーナスを選択する", "Choisissez votre bonus", "Pilih Bonusmu", "Bonus auswählen", "Scegli il tuo bonus", "选择您的奖励" };
    private string[] without_bonus_name = { "Without bonus", "Без бонуса", "Sem bónus", "Sin bonificación", "دون مكافأة", "बोनस के बिना", "Bonus olmadan dene", "ボーナスなし", "Sans bonus", "Tanpa bonus", "Ohne Bonus", "Senza bonus", "不要奖励" };
    private string[] settings_name = { "Settings","Настройки", "Definições", "Configuración", "إعدادات", "सेटिंग्स", "Ayarlar", "設定", "Paramètres", "Pengaturan", "Einstellungen", "Impostazioni", "设置" };
    private string[] close_name = { "Close","Назад", "Fechar", "Cerrar", "إغلاق", "बंद करें", "Kapat", "閉じる", "Fermer", "Tutup", "Schließen", "Chiudi", "关闭" };
    private string[] skins_name = { "Skins","Скины", "Skins", "Skins", "ألوان الأسلحة", "स्किन", "Kostümler", "スキン", "Skins", "Skin", "Skins", "Skin", "皮肤" };
    private string[] fly_name = { "Fly","Полет", "Voa", "Volar", "طِر", "उड़ें", "Uç", "飛ぶ", "Vol", "Terbang", "Fliegen", "Volo", "飞行" };
    private string[] double_range_lenght_name = {"Double Range Lenght","Двойная Длина", "Comprimento duplo de alcance", "Doble alcance del rango", "ضاعِف طول النطاق", "डबल रेंज की लंबाई", "Çift Uzunluk", "倍の距離範囲", "Double portée", "Dua Kali Jangkauan", "Doppelte Reichweite", "Lunghezza doppia gamma", "双倍长度", };
    private string[] next_name ={"Next", "Дальше", "Próximo", "Siguiente", "مقبل", "अगला", "Sonraki", "次へ", "Suivant", "Berikutnya", "Weiter", "Avanti", "下一页" };
    private string[] retry_name = { "Retry", "Ещe раз", "Tentativa", "Reintentar", "اعاده المحاوله", "पुनर्प्रयास", "Yeniden Dene", "リトライ", "Réessayer", "Coba lagi", "Wiederholung", "Riprova", "重试" };
    private string[] infinity_length_name ={  "Infinity Length", "Бесконечная длина", "Comprimento infinito", "Longitud infinita", "طول اللانهاية", "अनंत लंबाई", "Sonsuz uzunluk", "無限大の長さ", "Longueur de l'infini", "Panjang tak terhingga", "Unendliche Länge", "Lunghezza infinita", "无限长" };
    private string[] iron_skin_name= {  "Iron Skin", "Железная кожа", "Pele de Ferro", "Piel de hierro", "الجلد الحديدي", "आयरन स्किन", "Demir Deri", "アイアンスキン", "Peau de fer","Kulit Besi", "Eiserne Haut", "Pelle di ferro", "铁皮" };
    private string[] fly_info = {  "Get off all the walls and go up in the air", "Отцепись от всех стен и поднимайся по воздуху", "Afasta-te de todas as paredes e sobe pelo ar.",   "Deshazte de todas las paredes y sube por el aire",    "النزول جميع الجدران وترتفع في الهواء ",   "सभी दीवारों से उतरो और हवा में ऊपर जाओ",  "Tüm duvarlardan kurtul ve havaya çık",    "すべての壁を降りて、空気中に上がる",   "Détachez-vous de tous les murs et montez dans les airs",  "Dapatkan dari semua dinding dan naik di udara",   "Lass dich von allen Wänden los und steige durch die Luft",    "Scendi da tutte le pareti e sali in aria",    "离开所有的墙壁，升到空中"};
    private string[] double_info = {  "Your arms and legs stretch twice as much!", "Твои ручки и ножки растягиваются в два раза сильнее!", "Os teus braços e pernas esticam-se duas vezes mais!", "¡Tus brazos y piernas se estiran el doble!", "ذراعيك وساقيك تمتد مرتين!", "आपकी बाहों और पैरों में दो बार खिंचाव होता है!", "Kolların ve bacakların iki kat daha sıkı geriliyor!", "あなたの腕と脚は倍に伸びます！", "Tes bras et tes jambes s'étirent deux fois plus fort!", "Lengan dan kaki Anda meregang dua kali lipat!", "Deine Griffe und Beine dehnen sich doppelt so stark aus!", "Le braccia e le gambe si allungano due volte di più!", "你的手臂和腿伸展两倍！" };
    private string[] iron_info = {  "You won't be cut by sharp objects", "Тебя не разрежут острые предметы", "Você não será cortado por objetos pontiagudos", "No te cortarán objetos afilados", "لن يتم قطعك بواسطة أشياء حادة", "आप तेज वस्तुओं से नहीं कटेंगे", "Keskin nesnelerle kesilmeyeceksin", "あなたは鋭利なもので切断されることはありません", "Vous ne serez pas coupé avec des objets tranchants", "Anda tidak akan dipotong oleh benda tajam", "Du wirst nicht von scharfen Gegenständen aufgeschnitten", "Non sarai tagliato da oggetti appuntiti", "你不会被尖锐的物体割伤" };
    private string[] infinity_info = {  "Now you can stretch endlessly far!", "Теперь можешь тянуться бесконечно далеко!", "Agora você pode se esticar infinitamente mais!", "¡Ahora puedes estirarte infinitamente más!", "الآن يمكنك أن تمتد بعيدا بلا حدود!", "अब आप असीम रूप से दूर खींच सकते हैं!", "Şimdi sonsuza kadar uzanabilirsin!", "今、あなたは無限に遠くに伸ばすことができます！", "Maintenant, vous pouvez aller infiniment loin!", "Sekarang anda dapat meregangkan jauh jauh!", "Jetzt kannst du dich unendlich weit strecken!", "Ora puoi raggiungere infinitamente lontano!", "现在你可以无限伸展了！" };
    public int lang_i;
    public bool skins,fly, double_range_lenght,iron_skin,infinity_length,info;
    public TMP_FontAsset eng_font, ru_font, pt_font, es_font, ar_font, hi_font, tr_font, ja_font, fr_font, id_font, de_font, it_font, zh_font;
    [SerializeField] private LanguageSettings _settings;
    
    void Start()
    {
        var text = SceneManager.GetActiveScene().buildIndex;
        TMP_FontAsset[] fonts = { eng_font, ru_font, pt_font, es_font, ar_font, hi_font, tr_font, ja_font, fr_font, id_font, de_font, it_font, zh_font };
        if (_settings.IsLanguageOverriden)
        {
            test = _settings.OverrideLanguage;
        }
        else
        {
            test = LangExtern.GetLang();
        }
        Debug.Log(test);
        FindI();
        if (skins == false && fly == false && double_range_lenght == false&&iron_skin==false&&infinity_length==false)
        {
            hi_Skip.text = "";
            hi_ChooseYorBonus.text = "";
            hi_WithoutBonus.text = "";
            hi_Settings.text = "";
            hi_Close.text = "";
            hi_textName.text = "";
            hi_Next.text = "";
            hi_Retry.text = "";
            if (test != "hin")
            {
                Skip.font = fonts[lang_i];
                Close.font = fonts[lang_i];
                Settings.font = fonts[lang_i];
                WithoutBonus.font = fonts[lang_i];
                ChooseYorBonus.font = fonts[lang_i];
                Next.font = fonts[lang_i];
                Next.text = next_name[lang_i];
                Retry.font = fonts[lang_i];
                Retry.text = retry_name[lang_i];
                Skip.text = skip_name[lang_i];
                ChooseYorBonus.text = chouse_bonus_name[lang_i];
                WithoutBonus.text = without_bonus_name[lang_i];
                Settings.text = settings_name[lang_i];
                Close.text = close_name[lang_i];
                textName.font = fonts[lang_i];
            }
            if (test != "ar"&&test!="hni")
            {
                textName.text = (level_name[lang_i] + " " + text.ToString()).ToString();
            }
            if (test=="ar")
            {
                textName.text = (text.ToString() + " " + level_name[lang_i]).ToString();
            }
            if (test == "hin")
            {
                hi_Skip.text = skip_name[lang_i];
                hi_ChooseYorBonus.text = chouse_bonus_name[lang_i];
                hi_WithoutBonus.text = without_bonus_name[lang_i];
                hi_Settings.text = settings_name[lang_i];
                hi_Close.text = close_name[lang_i];
                hi_textName.text = (level_name[lang_i] + " " + text.ToString()).ToString();
                hi_Next.text = next_name[lang_i];
                hi_Retry.text = retry_name[lang_i];
                Retry.text = "";
                Skip.text = "";
                ChooseYorBonus.text = "";
                WithoutBonus.text = "";
                Settings.text = "";
                Close.text = "";
                textName.text = "";
                Next.text = "";
            }
        }
        if (skins == true)
        {
            if (test != "hni")
            {
                Skins.font = fonts[lang_i];
                Skins.text = skins_name[lang_i];
                hi_Skins.text = "";
            }
            if (test == "hni")
            {
                Skins.text = "";
                hi_Skins.text = skins_name[lang_i];
            }
        }
        if (fly == true)
        {
            if (test != "hin")
            {
                Fly.font = fonts[lang_i];
                Fly.text = fly_name[lang_i];
                if (info == true)
                    flyBord.text = fly_info[lang_i];
                // hi_Fly.text = "";
            }
            else
            {
                Fly.text = "";
                hi_Fly.text = fly_name[lang_i];
            }
        }
        if (double_range_lenght == true)
        {
            if (test != "hni")
            {
                DoubleRangeLenght.font = fonts[lang_i];
                DoubleRangeLenght.text = double_range_lenght_name[lang_i];
                if (info == true)
                    doubleBord.text = double_info[lang_i];
                // hi_DoubleRangeLenght.text = "";
            }
            else
            {
                DoubleRangeLenght.text = "";
                hi_DoubleRangeLenght.text = double_range_lenght_name[lang_i];
            }
        }
        if (iron_skin == true)
        {
            if (test != "hni")
            {
                IronSkin.font = fonts[lang_i];
                IronSkin.text = iron_skin_name[lang_i];
                if (info == true)
                    ironBord.text = iron_info[lang_i];
                //hi_IronSkin.text = "";
            }
            else
            {
                IronSkin.text = "";
                hi_IronSkin.text = iron_skin_name[lang_i];
            }
        }
        if (infinity_length == true)
        {
            if (test != "hni")
            {
                InfinityLength.font = fonts[lang_i];
                InfinityLength.text = infinity_length_name[lang_i];
                if (info == true)
                    infinityBord.text = infinity_info[lang_i];
                // hi_InfinityLength.text = "";
            }
            else
            {
                InfinityLength.text = "";
                //hi_InfinityLength.text = infinity_length_name[lang_i];
            }
        }

    }

    public void FindI()
    {
        for (int j = 0; j < 13; j++)
            if (test == lang[j])
            {
                lang_i = j;
                j = 13;
            }
    }
}
