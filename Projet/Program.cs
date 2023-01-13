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
            //On fait la saisie de l'utilisateur
            string saisie = SaisieUtilisateur();
            string resultat = "";
            //Si le datafriendly renvoie TRUE
            if(DataFriendly(ref maListe, saisie)){
                //On fait le calcul
                resultat = Calcul(maListe);
                //Si le calcul nous à renvoyé "/0", une division par zéro à été trouvé, donc on arrête le calcul car impossible de continuer
                if(resultat == "/0"){
                    Console.WriteLine("Division par zéro impossible !");
                }else{
                    //On affiche le résultat du calcul
                    affichageResultat(resultat);
                }
            }else{
                //On affiche à l'utilisateur que le calcul est impossible
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
            //Si la saisie est null
            if(saisie == null){
                //On la remplace par une chaine vide
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
            //Si la vérification de la saisie est correcte
            if(VerificationSaisie(saisie)){
                //On passe la saisie dans une liste
                maListe = SaisieEnListe(saisie);
                //Pour chaque élément de la liste
                foreach(string maChaine in maListe){
                    //S'il n'est pas bien constitué
                    if(!VerificationElement(maChaine)){
                        //On dit que c'est pas possible
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
            //Pour chaque caractères
            foreach (char caractere in saisie){
                //Si c'est pas un chiffre, ou un opérateur, ou une virgule ou un espace
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
            //On déclare nos booléens pour les tests
            //PM = PlusMoins
            //FD = FoisDivise
            bool operateurPM = false, operateurFD = false, nombre = false, virgule = false, resultat = true, passage = false;
            //Pour chaque element de l'élément à vérifier
            foreach (char cara in chaine)
            {
                //Si c'est la première étape, savoir si c'est un nombre ou un opérateur
                if (!operateurPM && !nombre && !operateurFD)
                {
                    //On effectue la vérification de phase 1
                    verificationPhase1(cara, ref nombre, ref operateurPM, ref operateurFD);
                    passage = true;
                }
                //Si les caractères prédents sont des nombres et qu'il n'y à pas eus de opérateurs divisé et fois et qu'il n'y a pas eus de pasage
                if ((nombre && !operateurFD) && !passage)
                {
                    //On vérifie si c'est un nombre
                    verificationSiNombre(cara, ref virgule, ref resultat);
                    passage = true;
                }
                //Si les caractères précédents ont été uniquement un opérateur plus ou moins
                if (!nombre && operateurPM && !operateurFD && !passage)
                {
                    //On regarde si l'élément actuel est un nombre un autre caractère
                    verificationSiOperateurPlusMoins(cara, ref nombre, ref resultat);
                    passage = true;
                }
                //Si toutes les étapes précédentes sont fausses et qu'il y a un opérateur de présent
                if ((operateurFD || operateurPM) && !passage)
                {
                    //on arrete la recherche
                    resultat = false;
                    passage = true;
                }
                passage = false;
            }
            return resultat;
        }

        /// <summary>
        /// Permets de vérifier la validité suite à un plus ou un moins
        /// </summary>
        /// <param name="cara"></param>
        /// <param name="nombre"></param>
        /// <param name="resultat"></param>
        static void verificationSiOperateurPlusMoins(char cara, ref bool nombre, ref bool resultat)
        {
            //Si le caractère est un nombre
            if ((int)cara <= 57 && (int)cara >= 48)
            {
                //On active le booléen de nombre
                nombre = true;
            }
            else
            {
                //On indique une erreur dans la saisie
                resultat = false;
            }
        }

        /// <summary>
        /// Permets de vérifier la validité suite à un nombre
        /// </summary>
        /// <param name="cara"></param>
        /// <param name="virgule"></param>
        /// <param name="resultat"></param>
        static void verificationSiNombre(char cara, ref bool virgule, ref bool resultat)
        {
            switch (cara)
            {
                //Si le caractère est un nombre
                case var x when (x <= 57 && x >= 48):
                    //On ne fait rien
                    break;
                //Si c'est une virgule
                case ',':
                    //Si c'est la deuxième virgule
                    if (virgule)
                    {
                        //L'élément n'est pas bon
                        resultat = false;
                    }
                    else
                    {
                        //On active la virgule
                        virgule = true;
                    }
                    break;
                default:
                    resultat = false;
                    break;
            }
        }

        /// <summary>
        /// Permets de déterminer le type de l'élément
        /// </summary>
        /// <param name="cara"></param>
        /// <param name="nombre"></param>
        /// <param name="operateurPM"></param>
        /// <param name="operateurFD"></param>
        static void verificationPhase1(char cara, ref bool nombre, ref bool operateurPM, ref bool operateurFD)
        {
            switch (cara)
            {
                //Si c'est un nombre
                case var x when (x <= 57 && x >= 48):
                    nombre = true;
                    break;
                //Si c'est un + ou un -
                case '+':
                case '-':
                    operateurPM = true;
                    break;
                //Si c'est un * ou un /
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
            //Création d'un type StringBuilder pour gérer les chaînes
            var myStringBuilder = new StringBuilder();
            foreach (char caractere in saisie){
                //Si on à plusieurs espaces de suite
                if (myStringBuilder.Equals("") && caractere == ' '){
                    myStringBuilder.Replace("", "");
                //Si on à un espace
                }else if (caractere == ' '){
                    //On ajoute à la liste les éléments précedent et on supprime le contenu du StringBuilder
                    ListeSaisie.Add(Convert.ToString(myStringBuilder));
                    myStringBuilder.Clear();
                }else{
                    myStringBuilder.Append(caractere);
                }
            //A la fin du traitement, si on à autre chose qu'un chaîne vide
            }if (!myStringBuilder.Equals("")){
                //On l'ajoute
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
            //Pour chaque éléments de la liste
            foreach (string chaine in maListe){
                //Pour chaque caractères de l'élément
                foreach (char caractere in chaine){
                    //Si c'est un chiffre
                    if ((int)caractere <= 57 && (int)caractere >= 48){
                        entier = true;
                    }
                }
                typeListe.Add(entier);
                entier = false;
            }
            int iteration = 2;
            //Parcours de la liste des types
            for (int i = 2; i < typeListe.Count; i++){
                //Si les itérations sont de 0
                if (iteration == 0){
                    return false;
                }
                //Si c'est un entier
                if (typeListe[i]){
                    //On ajoute
                    iteration++;
                }else{
                    //Sinon on enleve
                    iteration--;
                }
            }
            //S'il reste 1 élément, c'est bon !
            if (iteration == 1){
                return true;
            }
            //Sinon on retourne faux
            return false;
        }

        /// <summary>
        /// Permets de faire le calcul de la liste string passé en paramètre
        /// </summary>
        /// <param name="maListe"></param>
        /// <returns>Le résultat sous forme d'une string : Soit un nombre, Soit '/0' qui signifie que le programme à rencontré une division par 0, donc le calcul est impossible</returns>
        static string Calcul(List<string> maListe){
            List<string> listeNombre = new List<string>();
            //On empile les deux éléments vérifié
            empile(ref listeNombre, maListe[0]);
            empile(ref listeNombre, maListe[1]);
            //On démarre l'indice à 2
            int indice = 2;
            bool divisionZero = false;
            //Tant que l'indice est inférieur à la taille de la liste et qu'aucune division par zéro est arrivé
            while(indice < taille(maListe) && !divisionZero){
                //Si l'élément est un opérateur
                if (maListe[indice] == "-" || maListe[indice] == "+" || maListe[indice] == "*" || maListe[indice] == "/"){
                    //On dépile les deux nombres précédents
                    double nombreA = Convert.ToDouble(depile(ref listeNombre));
                    double nombreB = Convert.ToDouble(depile(ref listeNombre));
                    //Si les conditions d'une division par zéro sont réunis
                    if(nombreA == 0 && maListe[indice] == "/"){
                        //On active le drapeau
                        divisionZero = true;
                        //On enlève tous les éléments de la pile
                        while(taille(maListe) > 0){
                            string nombre = depile(ref maListe);
                        }
                        //On empile le caractère signalant une division par zéro
                        empile(ref listeNombre,"/0");
                    }else{
                        //On calcul et on empile
                        double calcul = CalculOperation(nombreB, nombreA, maListe[indice]);
                        empile(ref listeNombre, Convert.ToString(calcul));
                    }
                }else{
                    //On empile
                    empile(ref listeNombre, maListe[indice]);
                }
                //On augmente l'indice
                indice++;
            }
            //On dépile le dernier nombre, c'est à dire le résultat
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
            //On test l'opérateur
            switch(operateur){
                //Si addition
                case "+":
                    resultat = nombreA + nombreB;
                    break;
                //Si soustraction
                case "-":
                    resultat = nombreA - nombreB;
                    break;
                //Si multiplication
                case "*":
                    resultat = nombreA * nombreB;
                    break;
                //Si division
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
            //Si le paramètre est null
            if(maListe == null)
            {
                //On gère l'exception
                throw new ArgumentNullException(nameof(maListe));
            }
            //On récupère le nombre et on le supprime de la pile
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
            //Si l'un des paramètres est null, on gère l'exception
            if (maListe == null)
            {
                throw new ArgumentNullException(nameof(maListe));
            }
            if (nombre == null)
            {
                throw new ArgumentNullException(nameof(nombre));
            }
            //On ajoute le nombre à la liste
            maListe.Add(nombre);
        }

        /// <summary>
        /// Permets de calculer la taille d'une liste
        /// </summary>
        /// <param name="maListe"></param>
        /// <returns>La taille d'une liste</returns>
        static int taille(List<string> maListe){
            //Initialisation de l'indice à 0
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