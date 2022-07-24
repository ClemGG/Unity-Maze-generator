using System;

namespace Project.Procedural.MazeGeneration
{
    //Will create all the interfaces we need (IGeneration, IDraw & ISolwing) for the maze.
    public static class InterfaceFactory
    {
        //Dynamically creates the generation algorithm
        public static IGeneration GetGenerationAlgorithm(GenerationSettingsSO settings)
        {
            string algName;

            switch (settings.GenerationType)
            {
                case GenerationType.Random:
                    GenerationType[] types = Enums.ValuesOf<GenerationType>();
                    string[] typesNames = new string[types.Length-1];
                    for (int i = 1; i < types.Length; i++)  //Index 0 will always be Random, so we skip it
                    {
                        typesNames[i-1] = types[i].ToString();
                    }
                    algName = typesNames.Sample();
                    break;
                default:
                    algName = settings.GenerationType.ToString();
                    break;
            }

            Type algType = Type.GetType($"Project.Procedural.MazeGeneration.{algName}");

            //If the constructor is the default one (with no parameters),
            //we don't pass the settings to avoid missing the default constructor
            bool constructorHasParameters = algType.GetConstructors()[0].GetParameters().Length > 0;
            object[] parameters = constructorHasParameters ? new[] { settings } : null;
            IGeneration genAlg = (IGeneration)Activator.CreateInstance(algType, parameters);

            return genAlg;
        }

        //Dynamically creates the class to draw the maze on the screen
        public static IDraw GetDrawMode(GenerationSettingsSO settings)
        {

            Type algType = Type.GetType($"Project.Procedural.MazeGeneration.{settings.DrawMode}Draw");

            //If the constructor is the default one (with no parameters),
            //we don't pass the settings to avoid missing the default constructor
            bool constructorHasParameters = algType.GetConstructors()[0].GetParameters().Length > 0;
            object[] parameters = constructorHasParameters ? new[] { settings } : null;
            IDraw genAlg = (IDraw)Activator.CreateInstance(algType, parameters);

            return genAlg;
        }
    }
}