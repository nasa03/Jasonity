﻿using Assets.Code.Logic.AsSemantic;
using Assets.Code.Logic.AsSyntax;

namespace Assets.Code.Stdlib
{
    /*
     * Description: register the agent in the Directory Facilitator as a provider of service S of type T (see FIPA specification).
     * An optional second argument can be used to define the type of the service.
     */
    public class Df_Register: DefaultInternalAction
    {
        private static DefaultInternalAction singleton = null;
        public static DefaultInternalAction Create()
        {
            if (singleton == null)
            {
                singleton = new Df_Register();
            }
            return singleton;
        }

        public int GetMinArgs()
        {
            return 1;
        }

        public int GetMaxArgs()
        {
            return 2;
        }

        public object Excute(Reasoner ts, Unifier un, Term[] args)
        {
            CheckArguments(args);
            ts.GetUserAgArch().GetRuntimeServices().DfRegister(ts.GetUserAgArch().GetAgName(), GetService(args), GetType(args));
            return true;
        }

        protected string GetService(Term[] args)
        {
            if (args[0].IsString())
            {
                return (args[0] as StringTerm).GetString();
            }
            else
            {
                return args[0].ToString();
            }
        }

        protected string GetType(Term[] args)
        {
            if (args.Length > 1)
            {
                if (args[1].IsString())
                {
                    return (args[1] as StringTerm).GetString();
                }
                else if (!args[1].IsVar())
                {
                    return args[1].ToString();
                }
            }
            return "jason-type";
        }
    }
}