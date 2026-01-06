using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CinemaManagementSystem.AssociationClasses;

namespace CinemaManagementSystem
{
    [Serializable]
    public abstract class CleanableArea
    {
        private string _description;
        private TimeSpan _periodBetweenCleanings;

        public string Description
        {
            get => _description;
            protected set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Description cannot be empty.");
                _description = value;
            }
        }

        public TimeSpan PeriodBetweenCleanings
        {
            get => _periodBetweenCleanings;
            protected set
            {
                if (value < TimeSpan.Zero || value >= TimeSpan.FromDays(1))
                    throw new ArgumentException("Invalid cleaning period.");
                _periodBetweenCleanings = value;
            }
        }

        // ===== EXTENT (ALL CLEANABLE AREAS) =====
        [XmlIgnore]
        private static readonly List<CleanableArea> _areas = new();

        [XmlIgnore]
        public static IReadOnlyList<CleanableArea> Areas => _areas.AsReadOnly();

        protected void RegisterArea()
        {
            _areas.Add(this);
        }

        // ===== CLEANER ASSIGNMENTS =====
        [XmlIgnore]
        private readonly List<CleanerAssignment> _cleanerAssignments = new();

        [XmlIgnore]
        public IReadOnlyCollection<CleanerAssignment> CleanerAssignments
            => _cleanerAssignments.AsReadOnly();

        internal void AddCleanerAssignmentInternal(CleanerAssignment assignment)
        {
            if (assignment == null)
                throw new ArgumentNullException(nameof(assignment));

            _cleanerAssignments.Add(assignment);
        }

        internal void RemoveCleanerAssignmentInternal(CleanerAssignment assignment)
        {
            if (assignment == null)
                throw new ArgumentNullException(nameof(assignment));

            _cleanerAssignments.Remove(assignment);
        }

        [XmlIgnore]
        public bool IsNeedToBeCleaned
        {
            get
            {
                if (_cleanerAssignments.Count == 0)
                    return true;

                DateTime lastCleaning =
                    _cleanerAssignments.Max(a => a.CleaningDateTime);

                return DateTime.Now - lastCleaning > PeriodBetweenCleanings;
            }
        }

        public static List<CleanableArea> GenerateListOfAreaToClean()
        {
            return _areas.Where(a => a.IsNeedToBeCleaned).ToList();
        }

        protected CleanableArea() { }

        protected CleanableArea(string description, TimeSpan period)
        {
            Description = description;
            PeriodBetweenCleanings = period;
        }
    }
}
