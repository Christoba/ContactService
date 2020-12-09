namespace ContactService.Services
{
    using Models;

    /// <summary>
    /// The function configuration reader contract.
    /// </summary>
    public interface IFunctionConfigurationReader
    {
        /// <summary>
        /// Reads the configuration.
        /// </summary>
        /// <returns>
        /// The <see cref="IFunctionConfiguration"/>.
        /// </returns>
        IFunctionConfiguration Read();
    }
}
