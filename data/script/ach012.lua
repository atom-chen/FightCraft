--��ɫװ������
x100012_g_scriptId = 100012

--�¼�����
function x100012_OnEventFilter(uuid, achEventId, count)

	if count ~= nil and tonumber(count) <= LuaGetAllEquipsByQuality(uuid, 2) then
  		return 1
  	else
  		return 0
  	end
  	
end

--�ɾ���ɽ���
function x100012_OnAchievementProgress(uuid, reachNum)
  	return 1
end

