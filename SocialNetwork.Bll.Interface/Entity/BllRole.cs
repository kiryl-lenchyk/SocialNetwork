namespace SocialNetwork.Bll.Interface.Entity
{ 
    /// <summary>
    /// Represent role for work on business layer.
    /// </summary>
    public class BllRole
    {
        /// <summary>
        /// Unique identifier for role
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Role name as text unique identifier.
        /// </summary>
        public string Name { get; set; }

    }
}