﻿using Assets.Code.Logic.AsSyntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Represents an atom (a positive literal with no argument and no annotation, e.g. "tell", "a").
 */
namespace Assets.Code.Logic
{
    public class Atom: Literal
    {
        private static readonly long serialVersionUID = 1L;

        private readonly string functor; // immutable field
        private readonly Atom   ns; // name space

        public Atom(string functor): this(DefaultNS, functor)
        {
            
        }

        protected Atom(Atom @namespace, string functor)
        {
            if (functor == null)
            {
                
            }
            this.functor = functor;
            this.ns = @namespace;
        }

        public Atom(Literal l): this(l.GetNS(), l)
        {
            
        }

        public Atom(Literal l, Unifier u): this(l.GetNS().Capply(u) as Atom, l)
        {
            
        }

        public Atom(Atom @namespace, Literal l)
        {
            this.functor = l.GetFunctor();
            this.ns = @namespace;
            srcInfo = l.srcInfo;
        }

        public override string GetFunctor()
        {
            return functor;
        }

        public override Atom GetNS()
        {
            return this; //since this object is inmutable
        }

        public override Term Capply(Unifier u)
        {
            if (ns.IsVar())
            {
                return new Atom(this, u);
            }
            else
            {
                return this;
            }
        }

        public override Literal CloneNS(Atom newNamespace)
        {
            return new Atom(newNamespace, this);
        }

        public override bool IsAtom()
        {
            return true;
        }

        public override bool Equals(object o)
        {
            if (o == null) return false;
            if (o == this) return true;
            if (o.GetType() == typeof(Atom))
            {
                Atom a = o as Atom;
                return a.IsAtom() && GetFunctor().Equals(a.GetFunctor()) && GetNS().Equals(a.GetNS());
            }
            return false;
        }

        public override int CompareTo(Term t)
        {
            if (t == null) return -1; // null should be first (required for addAnnot)
            if (t.IsNumeric()) return 1;

            // this is a list and the other not
            if (IsList() && !t.IsList()) return -1;

            // this is not a list and the other is
            if (!IsList() && t.IsList()) return 1;

            // both are lists, check size
            if(IsList() && t.IsList())
            {
                ListTerm l1 = this as ListTerm;
                ListTerm l2 = t as ListTerm;
                const int l1s = l1.Size();
                const int l2s = l2.Size();

                if (l1s > l2s) return 1;
                if (l2s > l1s) return -1;
                return 0; //need to check elements
            }
            if (t.IsVar())
            {
                return -1;
            }
            if (t.GetType() == typeof(Literal))
            {
                Literal tAsList = t as Literal;
                if (GetNS().Equals(tAsList.GetNS()))
                {
                    const int ma = GetArity();
                    const int oa = tAsList.GetArity();
                    if (ma < oa)
                    {
                        return -1;
                    }
                    else if (ma > oa)
                    {
                        return 1;
                    }
                    else
                    {
                        return GetFunctor().CompareTo(tAsList.GetFunctor());
                    }
                }
                else
                {
                    return GetNS().CompareTo(tAsList.GetNS());
                }
            }
            return base.CompareTo(t);
        }

        protected override int? CalcHashCode()
        {
            return GetFunctor().GetHashCode() + GetNS().GetHashCode();
        }

        public override string ToString()
        {
            if (ns == DefaultNS)
            {
                return functor;
            }
            else
            {
                return GetNS() + "::" + functor;
            }
        }

        /** get as XML */
        /*
        public Element getAsDOM(Document document)
        {
            Element u = (Element)document.createElement("structure");
            u.setAttribute("functor", getFunctor());
            u.setAttribute("name-space", getNS().getFunctor());
            return u;
        }
        */
    }
}