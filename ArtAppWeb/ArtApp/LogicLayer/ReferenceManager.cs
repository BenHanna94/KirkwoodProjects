using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DataObjects;

namespace LogicLayer
{
    public class ReferenceManager : IReferenceManager
    {
        private IReferenceAccessor _referenceAccessor;

        public ReferenceManager()
        {
            _referenceAccessor = new ReferenceAccessor();
        }

        public ReferenceManager(ReferenceAccessor referenceAccessor)
        {
            _referenceAccessor = referenceAccessor;
        }

        public bool AddReference(Reference reference)
        {
            bool result = false;

            try
            {
                result = _referenceAccessor.InsertReference(reference) > 0;
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Reference Not Added", ex);
            }


            return result;
        }

        public bool DeleteReference(Reference reference)
        {
            bool result = false;
            try
            {
                result = (1 == _referenceAccessor.DeleteReference(reference));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Reference not deleted!", ex);
            }
            return result;
        }

        public bool EditReference(Reference oldReference, Reference newReference)
        {
            bool result = false;
            try
            {
                result = _referenceAccessor.UpdateReference(oldReference, newReference) == 1;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Data not found. ", ex);
            }

            return result;
        }

        /// <summary>
        /// Single reference
        /// </summary>
        /// <param name="reference"></param>
        public bool EditReference(Reference newReference)
        {
            bool result = false;
            try
            {
                Reference oldReference = _referenceAccessor.SelectReferenceByName(newReference.ReferenceName);
                result = _referenceAccessor.UpdateReference(oldReference, newReference) == 1;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Data not found. ", ex);
            }

            return result;
        }

        public List<Reference> GetReferencesByClient(int clientID)
        {
            try
            {
                return _referenceAccessor.GetReferencesByClient(clientID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Data not found. ", ex);
            }
        }

        public List<string> GetReferenceNamesByPiece(int pieceID)
        {
            List<string> refs = new List<string>();
            try
            {   
                List<Reference> references = _referenceAccessor.GetReferencesByPiece(pieceID);

                foreach (Reference reference in references)
                {
                    refs.Add(reference.ReferenceName);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("References not found. ", ex);
            }

            return refs;
        }

        public List<string> GetAllReferenceNames()
        {
            List<string> refs = new List<string>();
            try
            {
                List<Reference> references = _referenceAccessor.GetAllReferences();

                foreach (Reference reference in references)
                {
                    refs.Add(reference.ReferenceName);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("References not found. ", ex);
            }

            return refs;
        }

        public bool DeletePieceReference(int referenceID, int pieceID)
        {
            bool result = false;
            try
            {
                result = (1 == _referenceAccessor.InsertOrDeletePieceReference(referenceID, pieceID, delete: true));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Reference not removed!", ex);
            }
            return result;
        }

        public bool AddPieceReference(int referenceID, int pieceID)
        {
            bool result = false;
            try
            {
                result = (1 == _referenceAccessor.InsertOrDeletePieceReference(referenceID, pieceID));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Reference not added!", ex);
            }
            return result;
        }

        public Reference GetReferenceByName(string name)
        {
            Reference reference;
            try
            {
                reference = _referenceAccessor.SelectReferenceByName(name);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Reference not found!", ex);
            }

            return reference;
        }

        public List<Reference> GetAllReferences()
        {
            List<Reference> refs;
            try
            {
                refs = _referenceAccessor.GetAllReferences();
                
            }
            catch (Exception ex)
            {
                throw new ApplicationException("References not found. ", ex);
            }

            return refs;
        }

        public Reference GetReferenceByID(int id)
        {
            Reference reference;
            try
            {
                reference = _referenceAccessor.SelectReferenceByID(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Reference not found!", ex);
            }

            return reference;
        }
    }
}
