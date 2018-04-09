using System;
using System.Text;

namespace ISPCore.Engine.Common.Views
{
    public class SelectPickerTo
    {
        #region Int
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">true</param>
        /// <param name="FirstItem1">По домену</param>
        /// <param name="FirstItem2">Глобально</param>
        public static string Int(bool value, string FirstItem1, string FirstItem2) => Int(value, FirstItem1, FirstItem2, FirstItem2, FirstItem1);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">true</param>
        /// <param name="FirstItem1">Включена</param>
        /// <param name="FirstItem2">Отключить</param>
        /// <param name="LastItem1">Выключена</param>
        /// <param name="LastItem2">Включить</param>
        public static string Int(bool value, string FirstItem1, string FirstItem2, string LastItem1, string LastItem2)
        {
            if (value == true)
            {
                return $"<option value=\"1\" selected>{FirstItem1}</option><option value=\"0\">{FirstItem2}</option>";
            }

            return $"<option value=\"0\" selected>{LastItem1}</option><option value=\"1\">{LastItem2}</option>";
        }
        #endregion

        #region Bool
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">true</param>
        /// <param name="FirstItem1">По домену</param>
        /// <param name="FirstItem2">Глобально</param>
        public static string Bool(bool value, string FirstItem1, string FirstItem2) => Bool(value, FirstItem1, FirstItem2, FirstItem2, FirstItem1);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">true</param>
        /// <param name="FirstItem1">Включена</param>
        /// <param name="FirstItem2">Отключить</param>
        /// <param name="LastItem1">Выключена</param>
        /// <param name="LastItem2">Включить</param>
        public static string Bool(bool value, string FirstItem1, string FirstItem2, string LastItem1, string LastItem2)
        {
            if (value == true)
            {
                return $"<option value=\"true\" selected>{FirstItem1}</option><option value=\"false\">{FirstItem2}</option>";
            }

            return $"<option value=\"false\" selected>{LastItem1}</option><option value=\"true\">{LastItem2}</option>";
        }
        #endregion

        #region Enum
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">ConfToLog.'Текущее значение'</param>
        /// <param name="mass">(ConfToLog.'значение', Имя)</param>
        public static string Enum(Enum value, params (Enum value, string name)[] mass)
        {
            StringBuilder data = new StringBuilder();
            string SelectedValue = value.ToString();

            foreach (var item in mass)
            {
                string itemValue = item.value.ToString();
                data.Append($"<option value=\"{itemValue}\"{(itemValue == SelectedValue ? " selected" : "")}>{item.name}</option>");
            }

            return data.ToString();
        }
        #endregion
    }
}
