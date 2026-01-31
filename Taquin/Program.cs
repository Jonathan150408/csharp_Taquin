using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Taquin
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.White;

            //définition des variables générales
            bool leave = false;
            do
            {
                Console.Clear();
                switch (Menudemarrer())
                {
                    case 1:
                        Play();
                        break;
                    case 2:
                        Regles();
                        break;
                    case 3:
                        Environment.Exit(0);
                        break;
                }
            }while (!leave);
        }
        /// <summary>
        /// Demande le nombre de lignes et de colonnes pour la grille.
        /// </summary>
        /// <param name="direction">Nom de la direction (lignes/colonnes)</param>
        /// <param name="min">valeur minimale</param>
        /// <param name="max">valeur maximale</param>
        /// <returns>Nombre choisi par l'utilisateur</returns>
        static int AskingGridDimensions(string direction, int min, int max)
        {
            int dimension = 0;
            Console.WriteLine("Merci d'entrer le nombre de " + direction + " compris entre " + min + " et " + max);

            do
            {
                try
                {
                    dimension = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Il semblerais que la valeur entrée ne corresponde pas aux normes...");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Veuillez entrer un chiffre entier compris entre " + min + " et " + max + "\nVotre valeur : ");
                }
                if (dimension > max || dimension < min && dimension != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("La valeur entrée ne figure pas entre les bornes, veuillez entrer un chiffre entier compris entre " + min + " et " + max);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Votre valeur : ");
                    dimension = 0;
                }
            } while (dimension == 0);

            return dimension;
        }
        /// <summary>
        /// permet de jouer, affiche la grille et gère les déplacements des cases.
        /// </summary>
        static void Play()
        {
            //vide la console
            Console.Clear();

            //affiche le titre
            Title();

            //demande le nombre de lignes puis de colonnes
            int rows = (AskingGridDimensions("lignes", 2, 10));
            int columns = (AskingGridDimensions("colonnes", 2, 10));
            int[,] virtualGrid = GenerateGrid(rows, columns);

            //jouer la partie
            ConsoleKey keyPressed;
            char nomissclick;
            int moves = 0;
            int pos0X;
            int pos0Y;
            int posfullX = -1;
            int posfullY = -1;

            do
            {
                DrawGrid(rows, columns, virtualGrid, moves);

                keyPressed = Console.ReadKey(true).Key;

                //permet de déplacer la case vide
                if (keyPressed == ConsoleKey.UpArrow || keyPressed == ConsoleKey.DownArrow || keyPressed == ConsoleKey.LeftArrow || keyPressed == ConsoleKey.RightArrow)
                {
                    //trouver la case vide ( à déplacer)
                    pos0X = -2;
                    pos0Y = -2;
                    bool found = false;
                    for (int i = 0; i < rows && !found; i++)
                    {
                        for (int j = 0; j < columns && !found; j++)
                        {
                            if (virtualGrid[i, j] == 0)
                            {
                                pos0X = i;
                                pos0Y = j;
                                found = true;
                            }
                        }
                    }
                    //trouve la case à échanger
                    switch (keyPressed)
                    {
                        case ConsoleKey.LeftArrow:
                            posfullY = pos0Y - 1;
                            posfullX = pos0X;
                            break;
                        case ConsoleKey.RightArrow:
                            posfullY = pos0Y + 1;
                            posfullX = pos0X;
                            break;
                        case ConsoleKey.UpArrow:
                            posfullX = pos0X - 1;
                            posfullY = pos0Y;
                            break;
                        case ConsoleKey.DownArrow:
                            posfullX = pos0X + 1;
                            posfullY = pos0Y;
                            break;
                    }
                    //teste si la case est échangeable (dans les limites du tableau)
                    if (posfullX >= 0 && posfullX < columns && posfullY >= 0 && posfullY < rows)
                    {
                        //échange les valeurs
                        virtualGrid[pos0X, pos0Y] = virtualGrid[posfullX, posfullY];
                        virtualGrid[posfullX, posfullY] = 0;
                        moves++;
                    }                                   
                }

                //s'assure du choix (rejouer/quitter) de l'utilisateur
                if (keyPressed == ConsoleKey.Escape || keyPressed == ConsoleKey.R)
                {
                    Console.SetCursorPosition(50, 14);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Êtes-vous sûr de votre choix ? La partie en cours ne sera pas sauvegardée (O/N)");
                    Console.ForegroundColor = ConsoleColor.White;
                    nomissclick = Console.ReadKey(true).KeyChar;
                    if (nomissclick == 'O' || nomissclick == 'o')
                    {
                        break;
                    }
                    else
                    {
                        Console.SetCursorPosition(40, 14);
                        Console.Write("                                                                               ");
                        keyPressed = ConsoleKey.NoName; // réinitialise la touche pressée
                    }
                }
            } while (keyPressed != ConsoleKey.Escape && keyPressed != ConsoleKey.R && !Iswin(virtualGrid));

            if (Iswin(virtualGrid))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                //menu de fin
                int wannaPlay = 2;
                if (keyPressed == ConsoleKey.R)
                {
                    wannaPlay = 1;
                }
                else if (keyPressed == ConsoleKey.Escape)
                {
                    wannaPlay = 0;
                }
                //décide si on quitte ou pas, sans instructions on demande
                PlayAgain(wannaPlay, rows, columns, virtualGrid, moves);
                Console.Read();
            }

        }
        /// <summary>
        /// Génère la grille et la mélange
        /// </summary>
        /// <param name="rows">nombre de lignes du tableau</param>
        /// <param name="columns">nombre de colonnes par ligne</param>
        /// <returns>Renvoie la grille mélangée</returns>
        static int[,] GenerateGrid(int rows, int columns)
        {
            int[,] grid = new int[rows, columns];
            int count = 1;
            //crée la grille organisée
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    grid[i, j] = count;
                    count++;
                }
            }
            //set la dernière case sur 0
            grid[rows - 1, columns - 1] = 0;

            //mélange la grille, pour un 3x3 -> 450 mouvements
            Random random = new Random();
            int down;
            int right;

            for (int moves = 0; moves < rows * columns * 50; moves++)
            {
                //décide de la direction du mouvement
                int direction = random.Next(4);
                down = 0;
                right = 0;
                switch (direction)
                {
                    case 0:
                        //vers le haut
                        down = -1;
                        break;
                    case 1:
                        //vers la bas
                        down = 1;
                        break;
                    case 2:
                        //vers la gauche
                        right = -1;
                        break;
                    case 3:
                        //vers la droite
                        right = 1;
                        break;
                }

                //trouve la case 0
                int pos0X = -2;
                int pos0Y = -2;
                bool found = false;
                for (int i = 0; i < rows && !found; i++)
                {
                    for (int j = 0; j < columns && !found; j++)
                    {
                        if (grid[i, j] == 0)
                        {
                            pos0X = i;
                            pos0Y = j;
                            found = true;
                        }
                    }
                }
                //teste si la case est échangeable (dans les limites du tableau)
                if (pos0X + down >= 0 && pos0X + down < columns && pos0Y + right >= 0 && pos0Y + right < rows)
                {
                    //échange les valeurs
                    grid[pos0X, pos0Y] = grid[pos0X + down, pos0Y + right];
                    grid[pos0X + down, pos0Y + right] = 0;
                }
            }

            return grid;
        }
        /// <summary>
        /// Dessine la grille
        /// </summary>
        /// <param name="rows">nombre de lignes (axe Y/vertical)</param>
        /// <param name="columns">nombre de colonnes dans la grille (axe X/horizontal)</param>
        /// <param name="virtualGrid">Grille à représenter</param>
        static void DrawGrid(int rows, int columns, int[,] virtualGrid, int moves)
        {
            //dessine la grille
            Console.Clear();
            Title();
            //première ligne
            Console.Write("\t╔");
            for (int i = 0; i < columns - 1; i++)
            {
                Console.Write("═══╦");
            }
            Console.Write("═══╗");
            //ajoutes les valeurs en créant et complétant les lignes
            for (int i = 0; i < rows; i++)
            {
                Console.Write("\n\t║");
                for (int j = 0; j < columns; j++)
                {
                    if (virtualGrid[i, j] == 0)
                    {
                        //si la valeur est 0, on affiche un espace vide
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write("  ");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(" ║");
                    }
                    //la valeur ne prend qu'un seul caractère, on ajoute un espace avant
                    else if (virtualGrid[i, j] < 10)
                    {
                        Console.Write(" " + virtualGrid[i, j] + " ║");
                    }
                    //la valeur prend deux caractères
                    else if (virtualGrid[i, j] >= 10)
                    {
                        Console.Write(virtualGrid[i, j] + " ║");
                    }
                }
                //si ce n'est pas la dernière ligne, on affiche les lignes de séparation
                if (i != rows - 1)
                {
                    //affiche les lignes de séparation
                    Console.Write("\n\t╠");
                    for (int j = 0; j < columns - 1; j++)
                    {
                        Console.Write("═══╬");
                    }
                    Console.Write("═══╣");
                }
            }
            //dernière ligne
            Console.Write("\n\t╚");
            for (int i = 0; i < columns - 1; i++)
            {
                Console.Write("═══╩");
            }
            Console.Write("═══╝\n");

            //affiche le scoreboard
            Console.SetCursorPosition(50, 8);
            Console.WriteLine("Emploi :");
            Console.SetCursorPosition(50, 9);
            Console.WriteLine("Utilisez les flèches pour déplacer la case vide (en rouge)");
            Console.SetCursorPosition(50, 10);
            Console.WriteLine("Appuyez sur ESC pour quitter le jeu");
            Console.SetCursorPosition(50, 11);
            Console.WriteLine("Appuyez sur R pour recommencer une partie");
            Console.SetCursorPosition(50, 12);
            Console.WriteLine("-----------------------------");
            Console.SetCursorPosition(50, 13);
            Console.WriteLine("Mouvements effectués : " + moves);
        }
        /// <summary>
        /// Permet de rejouer ou de quitter le jeu.
        /// </summary>
        /// <param name="wannaPlay">0 on quitte, 1 on rejoue, 2 on demande</param>
        static void PlayAgain(int wannaPlay, int rows, int columns, int[,] grid, int moves)
        {
            Console.Clear();
            Title();
            DrawGrid(rows, columns, grid, moves);
            Console.WriteLine("Félicitations, défi remporté en " + moves + " mouvements.\nVoulez-vous rejouer ? (O/N)");
            ConsoleKey playNO = ConsoleKey.P;
            while (playNO != ConsoleKey.O && playNO != ConsoleKey.N && wannaPlay == 2)
            {
                playNO = Console.ReadKey(true).Key;
            }
            if (playNO == ConsoleKey.O || wannaPlay == 1)
            {
                Play();
            }
            else if (playNO == ConsoleKey.N || wannaPlay == 0)
            {
                Environment.Exit(0);
            }
        }
        /// <summary>
        /// vérifie si l'user a gagné
        /// </summary>
        /// <param name="grid">grille du puzzle</param>
        /// <returns>vrai = victoire, faux = pas encore gagné</returns>
        static bool Iswin(int[,] grid)
        {
            bool won = true;
            int count = 1;

            foreach (int x in grid)
            {
                if (x != count && count != grid.Length)
                {
                    won = false;
                    break;
                }
                else
                {
                    count++;
                }
            }

            return won;
        }
        /// <summary>
        /// Affiche le titre du jeu dans la console.
        /// </summary>
        static void Title()
        {
            Console.WriteLine("\t╔══════════════════════════════════════════════╗");
            Console.WriteLine("\t║                 - Taquin -                   ║");
            Console.WriteLine("\t║          Réalisé par Jonathan Junod          ║");
            Console.WriteLine("\t╚══════════════════════════════════════════════╝\n");
        }
        /// <summary>
        /// s'affiche lors du lancement du programme et permet de choisir le mode de jeu.
        /// </summary>
        static int Menudemarrer()
        {
            int choix = 1;
            int cursorline = 8;
            ConsoleKey didhemoved;

            Title();
            Console.CursorTop = 8;
            Console.WriteLine("Choississez l'option qui vous convient : \n\t1.Jouer \n\t2.Voir les règles\n\t3.Quitter");
            do
            {
                // Affiche la flèche du choix actuel
                Console.SetCursorPosition(5, cursorline + choix);
                Console.Write("->");
                didhemoved = Console.ReadKey(true).Key;
                // Efface la flèche du choix précédent
                Console.SetCursorPosition(5, cursorline + choix);
                Console.Write("  ");
                if (didhemoved == ConsoleKey.DownArrow)
                {
                    choix++;
                    // Si le choix est supérieur à 3, on le remet à 1
                    if (choix > 3)
                    {
                        choix = 1;
                    }
                }
                else if (didhemoved == ConsoleKey.UpArrow)
                {
                    choix--;
                    // Si le choix est inférieur à 1, on le remet à 3
                    if (choix < 1)
                    {
                        choix = 3;
                    }
                }
            } while (didhemoved != ConsoleKey.Enter);
            return choix;
        }
        /// <summary>
        /// Affiche les règles du jeu dans la console.
        /// </summary>
        static void Regles()
        {
            Console.Clear();
            Title();
            Console.WriteLine("Règles du jeu :\nLe but du jeu est de déplacer les cases pour reconstituer la suite de nombres.\nUtilisez les flèches directionnelles pour déplacer les cases.\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
    }
}
