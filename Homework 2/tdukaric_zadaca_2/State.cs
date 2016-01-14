// ***********************************************************************
// Assembly         : tdukaric_zadaca_2
// Author           : Tomislav
// Created          : 11-25-2013
//
// Last Modified By : Tomislav
// Last Modified On : 11-28-2013
// ***********************************************************************
// <copyright file="State.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The tdukaric_zadaca_2 namespace.
/// </summary>
namespace tdukaric_zadaca_2
{
    /// <summary>
    /// Class IState.
    /// </summary>
    abstract class IState
    {
        /// <summary>
        /// Gets or sets the prag.
        /// </summary>
        /// <value>The prag.</value>
        public int prag { get; set; }

        /// <summary>
        /// The n
        /// </summary>
        public int n;

        /// <summary>
        /// Update "slab"
        /// </summary>
        /// <param name="rank">The rank.</param>
        /// <param name="old_rank">The old_rank.</param>
        /// <returns>IState.</returns>
        public abstract IState Update(int rank, int old_rank);

    }

    /// <summary>
    /// Class thrown.
    /// </summary>
    class thrown : IState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="thrown" /> class.
        /// </summary>
        public thrown()
        {

        }
        /// <summary>
        /// Update "slab"
        /// </summary>
        /// <param name="rank">The rank.</param>
        /// <param name="old_rank">The old_rank.</param>
        /// <returns>IState.</returns>
        public override IState Update(int rank, int old_rank)
        {
            return this;
        }
    }

    /// <summary>
    /// Class weak.
    /// </summary>
    class weak : IState
    {


        /// <summary>
        /// Initializes a new instance of the <see cref="weak" /> class.
        /// </summary>
        /// <param name="prag">The prag.</param>
        public weak(int prag)
        {
            this.prag = prag;
            this.n = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="weak"/> class.
        /// </summary>
        /// <param name="c">The c.</param>
        public weak(IState c)
        {
            this.n = c.n;
        }

        /// <summary>
        /// Update "slab"
        /// </summary>
        /// <param name="rank">The rank.</param>
        /// <param name="old_rank">The old_rank.</param>
        /// <returns>IState.</returns>
        public override IState Update(int rank, int old_rank)
        {
            if (rank >= old_rank)
            {
                this.n++;
                if (this.prag <= this.n)
                {
                    return new thrown();
                }
            }
            else
                return new normal(this.prag);
            return this;
        }

    }

    /// <summary>
    /// Class normal.
    /// </summary>
    class normal : IState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="normal" /> class.
        /// </summary>
        /// <param name="prag">The n.</param>
        public normal(int prag)
        {
            this.prag = prag;
        }
        /// <summary>
        /// Update "normal"
        /// </summary>
        /// <param name="rank">The rank.</param>
        /// <param name="old_rank">The old_rank.</param>
        /// <returns>IState.</returns>
        public override IState Update(int rank, int old_rank)
        {
            if (rank > old_rank)
                return new weak(prag);
            else
                return this;
        }

    }

    /// <summary>
    /// Class Context.
    /// </summary>
    class Context
    {
        /// <summary>
        /// The state
        /// </summary>
        public IState state;

        /// <summary>
        /// Initializes a new instance of the <see cref="Context" /> class.
        /// </summary>
        /// <param name="state">The state.</param>
        public Context(IState state)
        {
            if (state is normal)
                this.state = new normal(state.prag);
            else if (state is thrown)
                this.state = new thrown();
            else
            {
                this.state = new weak(state);
            }
            
        }
        /// <summary>
        /// Updates the states.
        /// </summary>
        /// <param name="rank">The rank.</param>
        /// <param name="old_rank">The old_rank.</param>
        public void Update(int rank, int old_rank)
        {
            this.state = this.state.Update(rank, old_rank);
        }
    }
}
