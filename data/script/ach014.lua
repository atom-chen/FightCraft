--��ɫװ������
x100014_g_scriptId = 100014


--�¼�����
function x100014_OnEventFilter(uuid, achEventId, count)

	if count ~= nil and tonumber(count) <= LuaGetAllEquipsByQuality(uuid, 4) then
  		return 1
  	else
  		return 0
  	end
  	
end

--�ɾ���ɽ���
function x100014_OnAchievementProgress(uuid, reachNum)
  	return 1
end

