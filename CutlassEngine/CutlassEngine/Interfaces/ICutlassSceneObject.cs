namespace Cutlass.Interfaces
{
    public interface ICutlassSceneObject
    {
        /// <summary>
        /// Once this is set to false, will be removed from Scene.
        /// </summary>
        bool Active { get; set; }
    }
}
