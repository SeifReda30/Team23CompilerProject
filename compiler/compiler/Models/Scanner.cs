using System.Linq;
using System.Text.RegularExpressions;

namespace compiler.Models
{
    public class Scanner
    {
        int[,] TransationTable = new int[12, 9];

        public Scanner()
        {
            TransationTable[1,1] = 2;
            TransationTable[1, 2] = 4;
            TransationTable[1, 3] = 5;
            TransationTable[1, 4] = 6;
            TransationTable[1, 5] = 7;
            TransationTable[1, 6] = 2;
            TransationTable[1, 7] = 9;
            TransationTable[1, 8] = 1;
            TransationTable[2, 6] = 3;
            TransationTable[2, 8] = 11;
            TransationTable[3, 8] = 11;
            TransationTable[4, 2] = 4;
            TransationTable[4, 3] = 4;
            TransationTable[4, 8] = 11;
            TransationTable[5, 3] = 5;
            TransationTable[5, 8] = 11;
            TransationTable[6, 8] = 11;
            TransationTable[7, 5] = 8;
            TransationTable[8, 8] = 11;
            TransationTable[9, 6] = 10;
            TransationTable[10, 8] = 11;
            TransationTable[11, 8] = 1;
        }

        static string[] Signs = { "+", "-", "*", "/", "~", ".", "\"", "'", "{", "}", "[", "]", "(", ")" };

        public Dictionary<string, string> Keywords = new Dictionary<string, string>()
        {
            {"Program","Start statement"},
            {"End" ,"End Statement"},
            {"Ilap" , "Integer" },
            {"Silap","SInteger" },
            {"Ilapf","Float" },
            {"Silapf","SFlot" },
            {"Clop","Character" },
            {"Series","String" },
            {"Logical","Boolean" },
            {"If","Condition" },
            {"Else","Condition" },
            {"Check","Switch" },
            {"situationOf","Case of Switch" },
            {"Continuewhen","Loop" },
            {"Rotatewhen","Loop" },
            {"terminatethis","Break" },
            {"None","Void" },
            {"Replywith","Return" },
            {"Seop","Struct" },
            {"Category","Class" },
            {"Derive","Inheritance" },
            {"Using","Incluion" },
        };



        static string[] Letters = { "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q",
             "R","S","T","U","V","W","X","Y","Z","a","b","c","d","e","f","g","h","i","j","k","l","m","n",
             "o","p","q","r","s","t","u","v","w","x","y","z","_"
        };

        static string[] Digits = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };

        static string[] RelationalOperators = {"==", "<", ">", "<=", ">=", "!="};

        static string[] LogicOperators = { "&&", "||", "~"};

        static string[] ArthemiticOperators = { "+", "-", "*", "/"};

        static string AssignmentOperator = "=";

        static string AccessOperator = ".";

        static string[] QuotaionMark = { "\"", "'" };

        static string[] Braces = {"{", "}", "[", "]" , "(" , ")"};
        
        static int AcceptState=11;

        bool multicomment = false;


        public List<CorrectToken> correctTokens = new List<CorrectToken>();

        public List<WrongToken> wrongTokens = new List<WrongToken>();


        int lineNumber = 1;


        private void ReadFileLines(string fileSource,Scanner scanner)
        {
            // Read a text file line by line.  
            string[] lines = File.ReadAllLines(fileSource);

            foreach (string line in lines)
            {
                ReadingWords(line, scanner);
                lineNumber++;
            }
        }

        private static bool CheckifConstant(string word)
        {
            foreach (char item in word)
            {
                if (!Digits.Contains(item.ToString()))
                {
                    return false;
                }
            }
            return true;
        }

        public int GetNextState(int currentstate, char input)
        {
            int NextState;
            if (input == '<' || input == '>')
            {
                NextState = TransationTable[currentstate, 1];
            }
            else if (Letters.Contains(input.ToString()))
            {
                NextState = TransationTable[currentstate, 2];
            }
            else if (Digits.Contains(input.ToString()))
            {
                NextState = TransationTable[currentstate, 3];
            }
            else if (Signs.Contains(input.ToString()))
            {
                NextState = TransationTable[currentstate, 4];
            }
            else if (input == '&' || input == '|')
            {
                NextState = TransationTable[currentstate, 5];
            }
            else if (input == '=')
            {
                NextState = TransationTable[currentstate, 6];
            }
            else if (input == '!')
            {
                NextState = TransationTable[currentstate, 7];
            }
            else if (input == ' ' || input == ';' || input == '%' || input == '\t')
            {
                NextState = TransationTable[currentstate, 8];
            }
            else
            {
                NextState = 0;
            }
            return NextState;

        }
        private void ReadingWords(string line,Scanner scanner)
        {
            line += '%';
            int CurrentState = 1;
            int NextState;
            string token = "";
            int i;
            int length = line.Length;
            for (i=0;i<length;i++)
            {
                char input = line[i];
                if (multicomment)
                {
                    if (input == '*' && line[i + 1] == '>')
                    {
                        multicomment = false;
                        i += 1;
                        continue;
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (input == '-' && line[i + 1] == '-')
                {
                    return;
                }
                else if (input == '<' && line[i + 1] == '*')
                {
                    multicomment = true;
                    i += 1;
                    continue;
                }
                else
                {
                    NextState = GetNextState(CurrentState, input);

                    if (NextState==AcceptState)
                    {
                        CheckWord(token, lineNumber, scanner);
                        token = "";
                        CurrentState = 1;
                    }
                    else
                    {
                        if (NextState==0)
                        {
                            if (input==' ' || input == ';' || input== '%' || input == '\t')
                            {
                                WrongToken r = new WrongToken()
                                {
                                    LineNumber = lineNumber,
                                    Token = token
                                };

                                wrongTokens.Add(r);

                                token = "";
                                CurrentState = 1;
                            }
                            else
                            {
                                token += input;
                                CurrentState = NextState;

                            }
                        }
                        else
                        {
                            token += input;
                            CurrentState = NextState;

                        }

                    }

                }

            }         

        }
      

        private void CheckWord(string word, int lineNumber ,Scanner scanner)
        {
            string tokenValue = "";

            if (scanner.Keywords.Contains(word))
            {
                tokenValue = scanner.Keywords.getValue(word);
            }
            else if (Digits.Contains(word))
            {
                tokenValue = "Constant";
            }
            else if (RelationalOperators.Contains(word))
            {
                tokenValue = "Relational Operator";
            }
            else if (LogicOperators.Contains(word))
            {
                tokenValue = "Logic Operator";
            }
            else if (ArthemiticOperators.Contains(word))
            {
                tokenValue = "Arthemitic Operator";
            }
            else if (AssignmentOperator.Equals(word))
            {
                tokenValue = "Assignment Operator";
            }
            else if (AccessOperator.Equals(word))
            {
                tokenValue = "Access Operator";
            }
            else if (QuotaionMark.Contains(word))
            {
                tokenValue = "Quotaion Mark";
            }
            else if (Braces.Contains(word))
            {
                tokenValue = "Braces";
            }
            else if (CheckifConstant(word))
            {
                tokenValue = "Constant";
            }
            else
            {
                tokenValue = "Identifier";
            }


            CorrectToken r = new CorrectToken()
            {
               Token = word,
               TokenType = tokenValue,
               LineNumebr = lineNumber
            };

            correctTokens.Add(r);

        }



        public Scanner ScanningFile(String fileSource , Scanner scanner)
        {
            ReadFileLines(fileSource, scanner);
            
            return scanner;
        }
      

    }

}

