/*
 * ICloneable{TSelf}.cs
 *     Created: 2023-30-14T18:30:21-04:00
 *    Modified: 2024-29-19T13:29:23-04:00
 *      Author: David G. Moore, Jr. <david@dgmjr.io>
 *   Copyright: © 2022 - 2024 David G. Moore, Jr., All Rights Reserved
 *     License: MIT (https://opensource.org/licenses/MIT)
 */

namespace System;

/// <summary>
/// Clones the object
/// </summary>
/// <typeparam name="TSelf">The class' name</typeparam>
public interface ICloneable<TSelf>
    where TSelf : ICloneable<TSelf>
{
    /// <summary>
    /// Clones the object
    /// </summary>
    /// <returns>The cloned object</returns>
    TSelf Clone();
}
