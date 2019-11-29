/*
Copyright Since 2016 Akihiro Yamashita

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;

namespace OpenReceiptViewer
{
	/// <summary>型安全なコンバータ</summary>
	/// <typeparam name="T_UI">UIで必要な型</typeparam>
	/// <typeparam name="T_DATA">データの型</typeparam>
	public abstract class TypeSafeConverter<T_UI, T_DATA> : IValueConverter
	{
		/// <summary>値を変換します。</summary>
		/// <param name="value"></param>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public virtual T_UI Convert(T_DATA value, object parameter)
		{
			throw new NotImplementedException();
		}

		/// <summary>値を変換します。</summary>
		/// <param name="value"></param>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public virtual T_DATA ConvertBack(T_UI value, object parameter)
		{
			throw new NotImplementedException();
		}

		/// <summary>値を変換します。</summary>
		/// <param name="value">バインディング ソースによって生成された値。</param>
		/// <param name="targetType">バインディング ターゲット プロパティの型。</param>
		/// <param name="parameter">使用するコンバータ パラメータ。</param>
		/// <param name="culture">コンバータで使用するカルチャ。</param>
		/// <returns>変換された値。メソッドが null を返す場合は、有効な null 値が使用されています。</returns>
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
            // 型が違うし、継承関係もない。
            if (targetType != typeof(T_UI) && typeof(T_UI).IsSubclassOf(targetType) == false)
            {
                Debug.Assert(false);
                return DependencyProperty.UnsetValue;
            }
			if (value == DependencyProperty.UnsetValue)
			{
				return DependencyProperty.UnsetValue;
			}
			if (value != null)
			{
				Debug.Assert(value is T_DATA, string.Format("value({0}) is not instance of {1} but {2}", value, typeof(T_DATA), value.GetType()));
			}
			return Convert((T_DATA)value, parameter);
		}

		/// <summary>値を変換します。</summary>
		/// <param name="value">バインディング ターゲットによって生成される値。</param>
		/// <param name="targetType">変換後の型。</param>
		/// <param name="parameter">使用するコンバータ パラメータ。</param>
		/// <param name="culture">コンバータで使用するカルチャ。</param>
		/// <returns>変換された値。メソッドが null を返す場合は、有効な null 値が使用されています。</returns>
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			Debug.Assert(targetType == typeof(T_DATA));
			if (value != null)
			{
				Debug.Assert(value is T_UI, string.Format("value({0}) is not instance of {1} but {2}", value, typeof(T_UI), value.GetType()));
			}
			return ConvertBack((T_UI)value, parameter);
		}
	}

	/// <summary>型安全なマルチコンバータ</summary>
	/// <remarks>マルチといっても2個までしか対応していない。</remarks>
	/// <typeparam name="T_UI">UIで必要な型</typeparam>
	/// <typeparam name="T_DATA1">データ1の型</typeparam>
	/// <typeparam name="T_DATA2">データ2の型</typeparam>
	public abstract class TypeSafeMultiConverter<T_UI, T_DATA1, T_DATA2> : IMultiValueConverter
	{
		/// <summary>値を変換します。</summary>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public virtual T_UI Convert(T_DATA1 value1, T_DATA2 value2, object parameter)
		{
			throw new NotImplementedException();
		}

		/// <summary>値を変換します。</summary>
		/// <param name="value"></param>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public virtual Tuple<T_DATA1, T_DATA2> ConvertBack(T_UI value, object parameter)
		{
			throw new NotImplementedException();
		}

		/// <summary>値を変換します。</summary>
		/// <param name="values"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			Debug.Assert(values != null);
			Debug.Assert(values.Length == 2);
			Debug.Assert(targetType == typeof(T_UI));
			var value1 = values[0];
			var value2 = values[1];
			if (value1 == DependencyProperty.UnsetValue || value2 == DependencyProperty.UnsetValue)
			{
				return DependencyProperty.UnsetValue;
			}
			if (value1 != null)
			{
				Debug.Assert(value1 is T_DATA1, string.Format("value1({0}) is not instance of {1} but {2}", value1, typeof(T_DATA1), value1.GetType()));
			}
			if (value2 != null)
			{
				Debug.Assert(value2 is T_DATA2, string.Format("value2({0}) is not instance of {1} but {2}", value2, typeof(T_DATA2), value2.GetType()));
			}
			return Convert((T_DATA1)value1, (T_DATA2)value2, parameter);
		}

		/// <summary>値を変換します。</summary>
		/// <param name="value"></param>
		/// <param name="targetTypes"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			Debug.Assert(targetTypes != null);
			Debug.Assert(targetTypes.Length == 2);
			var targetType1 = targetTypes[0];
			var targetType2 = targetTypes[1];
			Debug.Assert(targetType1 == typeof(T_DATA1));
			Debug.Assert(targetType2 == typeof(T_DATA2));
			if (value != null)
			{
				Debug.Assert(value is T_UI, string.Format("value({0}) is not instance of {1} but {2}", value, typeof(T_UI), value.GetType()));
			}
			var tuple = ConvertBack((T_UI)value, parameter);
			return new object[] { tuple.Item1, tuple.Item2 };
		}
	}


	/// <summary>型安全なマルチコンバータ</summary>
	/// <remarks>マルチといっても3個までしか対応していない。</remarks>
	/// <typeparam name="T_UI">UIで必要な型</typeparam>
	/// <typeparam name="T_DATA1">データ1の型</typeparam>
	/// <typeparam name="T_DATA2">データ2の型</typeparam>
	/// <typeparam name="T_DATA3">データ3の型</typeparam>
	public abstract class TypeSafeMultiConverter<T_UI, T_DATA1, T_DATA2, T_DATA3> : IMultiValueConverter
	{
		/// <summary>値を変換します。</summary>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		/// <param name="value3"></param>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public virtual T_UI Convert(T_DATA1 value1, T_DATA2 value2, T_DATA3 value3, object parameter)
		{
			throw new NotImplementedException();
		}

		/// <summary>値を変換します。</summary>
		/// <param name="value"></param>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public virtual Tuple<T_DATA1, T_DATA2, T_DATA3> ConvertBack(T_UI value, object parameter)
		{
			throw new NotImplementedException();
		}

		/// <summary>値を変換します。</summary>
		/// <param name="values"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			Debug.Assert(values != null);
			Debug.Assert(values.Length == 3);
			Debug.Assert(targetType == typeof(T_UI));
			var value1 = values[0];
			var value2 = values[1];
			var value3 = values[2];
			if (value1 == DependencyProperty.UnsetValue || value2 == DependencyProperty.UnsetValue || value3 == DependencyProperty.UnsetValue)
			{
				return DependencyProperty.UnsetValue;
			}
			if (value1 != null)
			{
				Debug.Assert(value1 is T_DATA1, string.Format("value1({0}) is not instance of {1} but {2}", value1, typeof(T_DATA1), value1.GetType()));
			}
			if (value2 != null)
			{
				Debug.Assert(value2 is T_DATA2, string.Format("value2({0}) is not instance of {1} but {2}", value2, typeof(T_DATA2), value2.GetType()));
			}
			if (value3 != null)
			{
				Debug.Assert(value3 is T_DATA3, string.Format("value3({0}) is not instance of {1} but {2}", value3, typeof(T_DATA3), value3.GetType()));
			}
			return Convert((T_DATA1)value1, (T_DATA2)value2, (T_DATA3)value3, parameter);
		}

		/// <summary>値を変換します。</summary>
		/// <param name="value"></param>
		/// <param name="targetTypes"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			Debug.Assert(targetTypes != null);
			Debug.Assert(targetTypes.Length == 3);
			var targetType1 = targetTypes[0];
			var targetType2 = targetTypes[1];
			var targetType3 = targetTypes[2];
			Debug.Assert(targetType1 == typeof(T_DATA1));
			Debug.Assert(targetType2 == typeof(T_DATA2));
			Debug.Assert(targetType3 == typeof(T_DATA3));
			if (value != null)
			{
				Debug.Assert(value is T_UI, string.Format("value({0}) is not instance of {1} but {2}", value, typeof(T_UI), value.GetType()));
			}
			var tuple = ConvertBack((T_UI)value, parameter);
			return new object[] { tuple.Item1, tuple.Item2, tuple.Item3 };
		}
	}

	/// <summary>型安全なマルチコンバータ</summary>
	/// <remarks>マルチといっても4個までしか対応していない。</remarks>
	/// <typeparam name="T_UI">UIで必要な型</typeparam>
	/// <typeparam name="T_DATA1">データ1の型</typeparam>
	/// <typeparam name="T_DATA2">データ2の型</typeparam>
	/// <typeparam name="T_DATA3">データ3の型</typeparam>
	/// <typeparam name="T_DATA4">データ4の型</typeparam>
	public abstract class TypeSafeMultiConverter<T_UI, T_DATA1, T_DATA2, T_DATA3, T_DATA4> : IMultiValueConverter
	{
		/// <summary>値を変換します。</summary>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		/// <param name="value3"></param>
		/// <param name="value4"></param>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public virtual T_UI Convert(T_DATA1 value1, T_DATA2 value2, T_DATA3 value3, T_DATA4 value4, object parameter)
		{
			throw new NotImplementedException();
		}

		/// <summary>値を変換します。</summary>
		/// <param name="value"></param>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public virtual Tuple<T_DATA1, T_DATA2, T_DATA3, T_DATA4> ConvertBack(T_UI value, object parameter)
		{
			throw new NotImplementedException();
		}

		/// <summary>値を変換します。</summary>
		/// <param name="values"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			Debug.Assert(values != null);
			Debug.Assert(values.Length == 4);
			Debug.Assert(targetType == typeof(T_UI));
			var value1 = values[0];
			var value2 = values[1];
			var value3 = values[2];
			var value4 = values[3];
			if (value1 == DependencyProperty.UnsetValue || value2 == DependencyProperty.UnsetValue || value3 == DependencyProperty.UnsetValue || value4 == DependencyProperty.UnsetValue)
			{
				return DependencyProperty.UnsetValue;
			}
			if (value1 != null)
			{
				Debug.Assert(value1 is T_DATA1, string.Format("value1({0}) is not instance of {1} but {2}", value1, typeof(T_DATA1), value1.GetType()));
			}
			if (value2 != null)
			{
				Debug.Assert(value2 is T_DATA2, string.Format("value2({0}) is not instance of {1} but {2}", value2, typeof(T_DATA2), value2.GetType()));
			}
			if (value3 != null)
			{
				Debug.Assert(value3 is T_DATA3, string.Format("value3({0}) is not instance of {1} but {2}", value3, typeof(T_DATA3), value3.GetType()));
			}
			if (value4 != null)
			{
				Debug.Assert(value4 is T_DATA4, string.Format("value4({0}) is not instance of {1} but {2}", value4, typeof(T_DATA4), value4.GetType()));
			}
			return Convert((T_DATA1)value1, (T_DATA2)value2, (T_DATA3)value3, (T_DATA4)value4, parameter);
		}

		/// <summary>値を変換します。</summary>
		/// <param name="value"></param>
		/// <param name="targetTypes"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			Debug.Assert(targetTypes != null);
			Debug.Assert(targetTypes.Length == 4);
			var targetType1 = targetTypes[0];
			var targetType2 = targetTypes[1];
			var targetType3 = targetTypes[2];
			var targetType4 = targetTypes[3];
			Debug.Assert(targetType1 == typeof(T_DATA1));
			Debug.Assert(targetType2 == typeof(T_DATA2));
			Debug.Assert(targetType3 == typeof(T_DATA3));
			Debug.Assert(targetType4 == typeof(T_DATA4));
			if (value != null)
			{
				Debug.Assert(value is T_UI, string.Format("value({0}) is not instance of {1} but {2}", value, typeof(T_UI), value.GetType()));
			}
			var tuple = ConvertBack((T_UI)value, parameter);
			return new object[] { tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4 };
		}
	}

}
