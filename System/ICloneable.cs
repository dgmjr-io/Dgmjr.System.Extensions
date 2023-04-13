//
// ICloneable.cs
//
//   Created: 2022-10-23-11:39:49
//   Modified: 2022-11-11-06:11:07
//
//   Author: David G. Mooore, Jr. <david@dgmjr.io>
//
//   Copyright © 2022-2023 David G. Mooore, Jr., All Rights Reserved
//      License: MIT (https://opensource.org/licenses/MIT)
//


namespace System;

/// <summary>
/// Clones the object
/// </summary>
/// <typeparam name="TSelf">The class' name</typeparam>
public interface ICloneable<TSelf> where TSelf : ICloneable<TSelf>
{
    /// <summary>
    /// Clones the object
    /// </summary>
    /// <returns>The cloned object</returns>
    TSelf Clone();
}
