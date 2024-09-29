using UnityEngine;

namespace UnityExtension
{
    public enum Operator
    {
        Set,
        Add,
        Mult,
        Div
    }
    
    public static class OperatorOperation
    {
        public static float Calculate(this float currentValue, Operator @operator, float operatorValue)
        {
            float retval = Evaluate<float>(currentValue.ToString(), @operator, operatorValue.ToString());
            return retval;
        }
        
        public static int Calculate(this int currentValue, Operator @operator, int operatorValue)
        {
            int retval = Evaluate<int>(currentValue.ToString(), @operator, operatorValue.ToString());
            return retval;
        }
        
        private static T Evaluate<T>(string currentValue, Operator @operator, string operatorValue)
        {
            string formula = null;
            switch (@operator)
            {
                case Operator.Add:
                    formula = $"{currentValue}+{operatorValue}";
                    break;
                case Operator.Set:
                    formula = $"{operatorValue}";
                    break;
                case Operator.Div:
                    formula = $"{currentValue}/{operatorValue}";
                    break;
                case Operator.Mult:
                    formula = $"{currentValue}*{operatorValue}";
                    break;
            }
            T result = Evaluate<T>(formula);
            return result;
        }
        
        private static T Evaluate<T>(string formula)
        {
            ExpressionEvaluator.Evaluate<T>(formula, out T result);
            return result;
        }
    }
}