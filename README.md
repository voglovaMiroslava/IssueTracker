# IssueTracker
S použitím ASP.NET MVC vytvořte aplikaci pro sledování chyb a požadavků. 
Aplikace by měla dovolit evidenci zákazníků, zaměstnanců, projektů a jejich chyb nebo požadavků (Issues).

Způsob uložení dat: XML
Lze zobrazit, vytvářet a editovat zákazníky, osoby, projekty, chyby/požadavky a změny.
Entity jsou následující:
Zákazníkem je člověk nebo firma, která má alespoň jméno a kontaktní e-mailovou adresu.
Zaměstnanci jsou osoby řešící problémy.
Projekty jsou vázané zákazníka.
Ke každému projektu se eviduje seznam chyb a požadavků. 
Eviduje se typ (chyba/požadavek), stav (např. nový/řeší se/vyřešeno/zamítnuto), zadavatel (jméno člověka kdo nahlásil chybu), řešitel (zaměstnanec), datum zadání, datum dokončení.
Aplikace by, mimo jiné, měla:
Obsahovat přehled projektů, kde budou projekty rozděleny do skupiny podle zákazníka
V přehledu projektů zobrazit počty nezpracovaných/řešících se/dokončených chyb, a stejně tak i požadavků
Obsahovat přehled všech chyb a požadavků s možností filtru podle projektu, typu a stavu
V přehledu chybu/požadavků viditelně odlišovat jejich typ (chyba/požadavek) a stav
Ošetřovat vstupy uživatele a nedovolit chybné zadání údajů
Umožnit uživatelům vést u daného požadavku diskuzi
Umožnit uživateli zaregistrovat se ke sledování změn nad jakýmkoliv požadavkem a poskytnou nástroje pro správu aktivních notifikací.
Oznámení by se měla, jak zobrazovat v systému, tak být podle preferencí zasílána uživateli e-mailem.