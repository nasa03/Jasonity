﻿using Assets.Code.Agent;
using Assets.Code.AsSyntax;
using Assets.Code.ReasoningCycle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelExpr : BinaryStructure, ILogicalFormula
{
    private RelationalOp op = RelationalOp.none;
    public RelExpr(ITerm t1, RelationalOp oper, ITerm t2) : base(t1, oper.ToString(), t2) => op = oper;

    public IEnumerator<Unifier> LogicalConsequence(Agent.Agent ag, Unifier un)
    {
        ITerm xp = GetTerm(0).Capply(un);
        ITerm yp = GetTerm(1).Capply(un);

        if (op.GetType() == RelationalOp.none.GetType())
        {

        }

        switch (op.GetType())
        {
            case RelationalOp.none.GetType():
                break;

            case RelationalOp.gt:
                if (xp.CompareTo(yp) > 0) return LogExpr.CreateUnifEnumerator(un);
                break;
            case RelationalOp.gte:
                if (xp.CompareTo(yp) >= 0) return LogExpr.CreateUnifEnumerator(un);
                break;
            case RelationalOp.lt:
                if (xp.CompareTo(yp) < 0) return LogExpr.CreateUnifEnumerator(un);
                break;
            case RelationalOp.lte:
                if (xp.CompareTo(yp) <= 0) return LogExpr.CreateUnifEnumerator(un);
                break;
            case RelationalOp.eq:
                if (xp.Equals(yp)) return LogExpr.CreateUnifEnumerator(un);
                break;
            case RelationalOp.dif:
                if (!xp.Equals(yp)) return LogExpr.CreateUnifEnumerator(un);
                break;
            case RelationalOp.unify:
                if (un.Unifies(xp, yp)) return LogExpr.CreateUnifEnumerator(un);
                break;
            case RelationalOp.literalBuilder:
                try
                {
                    Literal p = (Literal)xp; // LHS clone
                    IListTerm l = (IListTerm)yp; // RHS clone

                    // Both are variables, using normal unification
                    if (!p.IsVar() && !l.IsVar())
                    {
                        IListTerm palt = p.GetAsListOfTerms();
                        if (l.Count == 3) // list without name space
                            palt = palt.GetNext();
                        if (un.Unifies(palt, l))
                            return LogExpr.CreateUnifEnumerator(un);
                    }
                    else
                    {
                        // First is var, second is list, var is assigned to l transformed into literal
                        if (p.IsVar() && l.IsList())
                        {
                            ITerm t = null;
                            if (l.Count == 4 && l[3].IsPlanBody()) // The list is for a plan
                                t = Plan.NewFromListOfTerms(l);
                            else t = Literal.NewFromListOfTerms(l);
                            if (un.Unifies(p, t)) return LogExpr.CreateUnifEnumerator(un);
                            else LogExpr.EMPTY_UNIF_LIST.GetEnumerator();
                        }

                        // First is literal, second is var, var is assigned to l transformed in list
                        if (p.IsLiteral() && l.IsVar())
                        {
                            if (un.Unifies(p.GetAsListOfTerms(), l)) return LogExpr.CreateUnifEnumerator(un);
                            else LogExpr.EMPTY_UNIF_LIST.GetEnumerator();
                        }
                    }
                }
                catch (Exception e)
                {
                    
                }
                break;
        }
        return LogExpr.EMPTY_UNIF_LIST.GetEnumerator();
    }

    public override ITerm Capply(Unifier u) => new RelExpr(GetTerm(0).Capply(u), op, GetTerm(1).Capply(u));

    // Make a hard copy of the terms
    public new ILogicalFormula Clone() => new RelExpr(GetTerm(0).Clone(), op, GetTerm(1).Clone());

    public override Literal CloneNS(Atom newnamespace) => new RelExpr(GetTerm(0).CloneNS(newnamespace), op, GetTerm(1).CloneNS(newnamespace));

    public RelationalOp GetOp() => op;

    public class RelationalOp
    {
        public readonly static RelationalOp none;
        public readonly static RelationalOp gt;
        public readonly static RelationalOp gte;
        public readonly static RelationalOp lt;
        public readonly static RelationalOp lte;
        public readonly static RelationalOp eq;
        public readonly static RelationalOp dif;
        public readonly static RelationalOp unify;
        public readonly static RelationalOp literalBuilder;

        public override string ToString()
        {
            if (this == none) return "";
            else if (this == gt) return " > ";
            else if (this == gte) return " >= ";
            else if (this == lt) return " < ";
            else if (this == lte) return " <= ";
            else if (this == eq) return " == ";
            else if (this == dif) return " \\== ";
            else if (this == unify) return " = ";
            else if (this == literalBuilder) return " =.. ";
            else return null;
        }
    }
}
