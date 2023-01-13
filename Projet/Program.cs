//DUBUT Florent
//Premiere version : 06/01/23
//Dernière version : 12/01/23


using System;
using System.Text;

namespace Projet
{
    static class Projet
    {
        static void Main(){
            List<string> maListe = new List<string>();
            string saisie = SaisieUtilisateur();
            string resultat = "0";
            if(DataFriendly(ref maListe, saisie)){
                resultat = Calcul(maListe);
                if(resultat == "/0"){
                    Console.WriteLine("Division par zéro impossible !");
                }else{
                    affichageResultat(resultat);
                }
            }else{
                Console.WriteLine("Le calcul est impossible !");
            }
        }

        /// <summary>
        /// Permets d'effectuer la saisie de l'utilisateur
        /// </summary>
        /// <returns>La chaîne saisit par l'utilisateur</returns>
        static string SaisieUtilisateur(){
            Console.Write("Ecrire votre calcul : ");
            string saisie = Console.ReadLine();
            if(saisie == null){
                saisie = "";
            }
            return saisie;
        }

        /// <summary>
        /// Permets de vérifier la validité de la saisie
        /// </summary>
        /// <param name="maListe"></param>
        /// <param name="saisie"></param>
        /// <returns>La liste passé en référence et un booléen</returns>
        static bool DataFriendly(ref List<string> maListe, string saisie){
            bool resultat = true;
            if(VerificationSaisie(saisie)){
                maListe = SaisieEnListe(saisie);
                foreach(string maChaine in maListe){
                    if(!VerificationElement(maChaine)){
                        resultat = false;
                    }
                }
                if (!VerificationCalcul(maListe)){
                    resultat = false;
                }
            }
            else{
                resultat = false;
            }
            return resultat;
        }

        /// <summary>
        /// Permet de vérifier tous les caractères de la saisie
        /// </summary>
        /// <param name="saisie"></param>
        /// <returns>Un booléen : True si la saisie est bonne, False s'il est incorrecte</returns>
        static bool VerificationSaisie(string saisie){
            bool verification = true;
            foreach (char caractere in saisie){
                if (((int)caractere < 48 || (int)caractere > 57) && caractere != '-' && caractere != '+' && caractere != '*' && caractere != '/' && caractere != ' ' && caractere != ','){
                    verification = false;
                }
            }
            return verification;
        }

        /// <summary>
        /// Permets de vérifier l'élément en paramètre
        /// </summary>
        /// <param name="chaine"></param>
        /// <returns>True si l'élément est correcte, False s'il est incorrecte</returns>
        static bool VerificationElement(string chaine)
        {
            bool operateurPM = false, operateurFD = false, nombre = false, virgule = false, resultat = true, passage = false;
            foreach (char cara in chaine)
            {
                if (!operateurPM && !nombre && !operateurFD)
                {
                    verificationPhase1(cara, ref nombre, ref operateurPM, ref operateurFD);
                    passage = true;
                }
                if ((nombre && !operateurFD) && !passage)
                {
                    verificationSiNombre(cara, ref virgule, ref resultat);
                    passage = true;
                }
                if (!nombre && operateurPM && !operateurFD && !passage)
                {
                    verificationSiOperateurPlusMoins(cara, ref nombre, ref resultat);
                    passage = true;
                }
                if ((operateurFD || operateurPM) && !passage)
                {
                    resultat = false;
                    passage = true;
                }
                passage = false;
            }
            return resultat;
        }

        static void verificationSiOperateurPlusMoins(char cara, ref bool nombre, ref bool resultat)
        {
            if ((int)cara <= 57 && (int)cara >= 48)
            {
                nombre = true;
            }
            else
            {
                resultat = false;
            }
        }

        static void verificationSiNombre(char cara, ref bool virgule, ref bool resultat)
        {
            switch (cara)
            {
                case var x when (x <= 57 && x >= 48):
                    break;
                case ',':
                    if (virgule)
                    {
                        resultat = false;
                    }
                    else
                    {
                        virgule = true;
                    }
                    break;
                default:
                    resultat = false;
                    break;
            }
        }

        static void verificationPhase1(char cara, ref bool nombre, ref bool operateurPM, ref bool operateurFD)
        {
            switch (cara)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    nombre = true;
                    break;
                case '+':
                case '-':
                    operateurPM = true;
                    break;
                case '*':
                case '/':
                    operateurFD = true;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Permets de passer un string vers une liste de string en fonction des espaces
        /// </summary>
        /// <param name="saisie"></param>
        /// <returns>Une liste de string</returns>
        static List<string> SaisieEnListe(string saisie){
            List<string> ListeSaisie = new List<string>();
            //Transformation en liste
            var myStringBuilder = new StringBuilder();
            foreach (char caractere in saisie){
                //Si on à plusieurs espaces de suite
                if (myStringBuilder.Equals("") && caractere == ' '){
                    myStringBuilder.Replace("", "");
                }else if (caractere == ' '){
                    ListeSaisie.Add(Convert.ToString(myStringBuilder));
                    myStringBuilder.Clear();
                }else{
                    myStringBuilder.Append(caractere);
                }
            }if (!myStringBuilder.Equals("")){
                ListeSaisie.Add(Convert.ToString(myStringBuilder));
            }
            return ListeSaisie;
        }

        /// <summary>
        /// Permets de vérifier la validité du calcul
        /// </summary>
        /// <param name="maListe"></param>
        /// <returns>True si le calcul est possible, False s'il est impossible</returns>
        static bool VerificationCalcul(List<string> maListe){
            bool entier = false;
            List<bool> typeListe = new List<bool>();
            foreach (string chaine in maListe){
                foreach (char caractere in chaine){
                    if ((int)caractere <= 57 && (int)caractere >= 48){
                        entier = true;
                    }
                }
                typeListe.Add(entier);
                entier = false;
            }
            int iteration = 2;
            for (int i = 2; i < typeListe.Count; i++){
                if (iteration == 0){
                    return false;
                }
                if (typeListe[i]){
                    iteration++;
                }else{
                    iteration--;
                }
            }
            if (iteration == 1){
                return true;
            }
            return false;
        }

        /// <summary>
        /// Permets de faire le calcul de la liste string passé en paramètre
        /// </summary>
        /// <param name="maListe"></param>
        /// <returns>Le résultat sous forme d'une string : Soit un nombre, Soit '/0' qui signifie que le programme à rencontré une division par 0, donc le calcul est impossible</returns>
        static string Calcul(List<string> maListe){
            List<string> listeNombre = new List<string>();
            empile(ref listeNombre, maListe[0]);
            empile(ref listeNombre, maListe[1]);
            int indice = 2;
            bool divisionZero = false;
            while(indice < taille(maListe) && !divisionZero){
                if (maListe[indice] == "-" || maListe[indice] == "+" || maListe[indice] == "*" || maListe[indice] == "/"){
                    double nombreA = Convert.ToDouble(depile(ref listeNombre));
                    double nombreB = Convert.ToDouble(depile(ref listeNombre));
                    if(nombreA == 0 && maListe[indice] == "/"){
                        divisionZero = true;
                        while(taille(maListe) > 0){
                            string nombre = depile(ref maListe);
                        }
                        empile(ref listeNombre,"/0");
                    }else{
                        double calcul = CalculOperation(nombreB, nombreA, maListe[indice]);
                        empile(ref listeNombre, Convert.ToString(calcul));
                    }
                }else{
                    empile(ref listeNombre, maListe[indice]);
                }
                indice++;
            }
            return depile(ref listeNombre);     
        }

        /// <summary>
        /// Permets de faire le calcul avec les deux nombre et l'opérateur passé en paramètre
        /// </summary>
        /// <param name="nombreA"></param>
        /// <param name="nombreB"></param>
        /// <param name="operateur"></param>
        /// <returns>Retourne le résultat de l'opération</returns>
        static double CalculOperation(double nombreA, double nombreB, string operateur){
            double resultat = 0;
            switch(operateur){
                case "+":
                    resultat = nombreA + nombreB;
                    break;
                case "-":
                    resultat = nombreA - nombreB;
                    break;
                case "*":
                    resultat = nombreA * nombreB;
                    break;
                default:
                    resultat = nombreA / nombreB;
                    break;
            }
            return resultat;
        }

        /// <summary>
        /// Permets de dépiler une liste
        /// </summary>
        /// <param name="maListe"></param>
        /// <returns>L'élément dépilé sous forme d'un string</returns>
        static string depile(ref List<string> maListe){
            if(maListe == null)
            {
                throw new ArgumentNullException("maListe");
            }

            string nombre = maListe[maListe.Count - 1];
            maListe.RemoveAt(maListe.Count - 1);
            return nombre;
        }

        /// <summary>
        /// Permets d'empiler dans une liste
        /// </summary>
        /// <param name="maListe"></param>
        /// <param name="nombre"></param>
        static void empile(ref List<string> maListe,string nombre){
            if (maListe == null)
            {
                throw new ArgumentNullException("maListe");
            }
            if (nombre == null)
            {
                throw new ArgumentNullException("nombre");
            }
            maListe.Add(nombre);
        }

        /// <summary>
        /// Permets de calculer la taille d'une liste
        /// </summary>
        /// <param name="maListe"></param>
        /// <returns>La taille d'une liste</returns>
        static int taille(List<string> maListe){
            int indice = 0;
            foreach(string element in maListe){
                indice++;
            }
            return indice;
        }

        /// <summary>
        /// Permets de faire l'affichage du résultat du calcul
        /// </summary>
        /// <param name="nombre"></param>
        static void affichageResultat(string nombre){
            Console.WriteLine("Le résultat du calcul fait : " + nombre);
        }
    }
}