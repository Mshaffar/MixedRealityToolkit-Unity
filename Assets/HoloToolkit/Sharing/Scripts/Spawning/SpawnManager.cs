﻿//
// Copyright (C) Microsoft. All rights reserved.
// TODO This needs to be validated for HoloToolkit integration
//

using HoloToolkit.Sharing.SyncModel;
using UnityEngine;

namespace HoloToolkit.Sharing.Spawning
{
    /// <summary>
    /// A SpawnManager is in charge of spawning the appropriate objects based on changes to an array of data model objects
    /// to which it is registered.
    /// It also manages the lifespan of these spawned objects.
    /// </summary>
    /// <typeparam name="T">Type of SyncObject in the array being monitored by the SpawnManager.</typeparam>
    public abstract class SpawnManager<T> : MonoBehaviour where T : SyncObject, new()
    {
        protected SharingStage NetworkManager { get; private set; }

        protected SyncArray<T> SyncSource;

        protected virtual void Start()
        {
            NetworkManager = SharingStage.Instance;
            SetDataModelSource();
            RegisterToDataModel();
        }

        /// <summary>
        /// Sets the data model source for the spawn manager.
        /// </summary>
        protected abstract void SetDataModelSource();

        /// <summary>
        /// Register to data model updates;
        /// </summary>
        private void RegisterToDataModel()
        {
            this.SyncSource.ObjectAdded += OnObjectAdded;
            this.SyncSource.ObjectRemoved += OnObjectRemoved;
        }

        private void OnObjectAdded(T addedObject)
        {
            InstantiateFromNetwork(addedObject);
        }

        private void OnObjectRemoved(T removedObject)
        {
            RemoveFromNetwork(removedObject);
        }

        /// <summary>
        /// Delete the data model for an object and all its related game objects.
        /// </summary>
        /// <param name="objectToDelete">Object that needs to be deleted.</param>
        public abstract void Delete(T objectToDelete);

        /// <summary>
        /// Instantiate game objects based on data model that was created on the network.
        /// </summary>
        /// <param name="addedObject">Object that was added to the data model.</param>
        protected abstract void InstantiateFromNetwork(T addedObject);

        /// <summary>
        /// Remove an object based on data model that was removed on the network.
        /// </summary>
        /// <param name="removedObject">Object that was removed from the data model.</param>
        protected abstract void RemoveFromNetwork(T removedObject);
    }
}
