﻿using Assets.Code.Logic;
using Assets.Code.Logic.AsSemantic;
using Assets.Code.Logic.AsSyntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Description: concatenates strings or lists.
 */
namespace Assets.Code.Stdlib
{
    public class Concat: DefaultInternalAction
    {
        private static InternalAction singleton = null;
        public static InternalAction Create()
        {
            if(singleton == null)
            {
                singleton = new Concat();
            }
            return singleton;
        }

        public override object Execute(Reasoner ts, Unifier un, Term[] args)
        {
            if (args[0].IsList())
            {
                if (!args[args.Length-1].IsVar() && !args[args.Length-1].IsList())
                {
                    throw new JasonException("Last argument of concat '" + args[args.Length - 1] + "'is not a list nor a variable.");
                }
                ListTerm result = args[0].Clone() as ListTerm;
                for (int i = 1; i < args.Length - 1; i++)
                {
                    if (!args[i].IsList())
                    {
                        throw JasonException.createWrongArgument(this, "arg[" + i + "] is not a list");
                    }
                    return un.Unifies(result, args[args.Length - 1]);
                }
            }
            else
            {
                if (!args[args.Length - 1].IsVar() && !args[args.Length - 1].IsString())
                {
                    throw JasonException.createWrongArgument(this, "Last argument '" + args[args.Length - 1] + "' is not a string nor a variable.");
                }
                string vl = args[0].ToString();
                if (args[0].IsString())
                {
                    vl = args[0].GetString() as StringTerm;
                }
                StringBuilder sr = new StringBuilder(vl);
                for (int i = 0; i < args.Length-1; i++)
                {
                    vl = args[i].ToString();
                    if (args[i].IsString())
                    {
                        vl = args[i].GetString as StringTerm;
                    }
                    sr.Append(vl);
                }
                return un.Unifies(new StringTermImpl(sr.ToString()), args[args.Length-1]);
            }
        }
    }
}